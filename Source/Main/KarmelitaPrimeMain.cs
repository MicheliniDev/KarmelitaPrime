using System;
using BepInEx;
using UnityEngine;
using HarmonyLib;
using System.IO;
using System.Reflection;
using System.Collections;
using BepInEx.Configuration;
using KarmelitaPrime.Patches;
using KarmelitaPrime.Drawers;
using UnityEngine.Networking;

namespace KarmelitaPrime;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class KarmelitaPrimeMain : BaseUnityPlugin
{
    public static KarmelitaPrimeMain Instance;
    
    public Texture2D[] CurrentTextures = new Texture2D[2];
    public Texture2D[] WhatsappTextures = new Texture2D[2];
    public Texture2D[] KarmelitaTextures = new Texture2D[2];
    public AudioClip WindBladeCueSound;
    public AudioClip WhatsappSound;
    private Texture2D goodTimeFrog;
    
    public goodtimefrog Goodtimefrog;
    
    private Harmony harmony;
    public KarmelitaWrapper wrapper;
    
    private ConfigEntry<bool> fightBoss;
    public ConfigEntry<bool> IsDebug;
    public ConfigEntry<bool> IsHitless;
    public ConfigEntry<bool> isWhatsapp;

    public Font MenuFont;
    public Shader FlashShader;
    
    public void Awake()
    {
        Logger.LogInfo($"{MyPluginInfo.PLUGIN_NAME} is loaded");
        //harmony = Harmony.CreateAndPatchAll(typeof(KarmelitaPrimeMain).Assembly);
        fightBoss = Config.Bind("", "Fight the Boss", false,
            new ConfigDescription(
                "Click here to fight the boss.",
                null,
                new ConfigurationManagerAttributes
                {
                    CustomDrawer = BossButtonDrawer.DrawButton,
                    HideDefaultButton = true,
                    ReadOnly = true,
                    HideSettingName = true
                }
            ));
        //IsDebug = Config.Bind("Settings", "IsDebug", false, "PLAY EDUARDO'S AWAKENING :D");
        isWhatsapp = Config.Bind("Whatsapp", "Whatsapp", false, "Whatsapp");
        IsHitless = Config.Bind("Challenges", "Hitless", false, "If enabled, any hit taken will instantly kill Hornet");

        StartCoroutine(WaitUntilGameManager());
        
        BossButtonDrawer.OnButtonPressed += TeleportToKarmelitaScene;
        isWhatsapp.SettingChanged += OnWhatsappSet;
        
        LoadEmbeddedResources();
        
        Instance = this;
    }
    
    private IEnumerator WaitUntilGameManager()
    {
        yield return new WaitUntil(() => GameManager.instance);
        yield return null;
        GetResources();
        harmony = Harmony.CreateAndPatchAll(typeof(KarmelitaPrimeMain).Assembly);
        yield return null;
    }

    private void GetResources()
    {
        foreach (var font in Resources.FindObjectsOfTypeAll<Font>())
        {
            if (!font.name.Contains("Trajan")) continue;
            MenuFont = font;
            break;
        }
        foreach (var shader in Resources.FindObjectsOfTypeAll<Shader>())
        {
            if (!shader.name.Contains("Sprites/Default-ColorFlash")) continue;
            FlashShader = shader;
            break;
        }
    }
    
    private void LoadEmbeddedResources() {
        var assembly = Assembly.GetExecutingAssembly();
        foreach (string resourceName in assembly.GetManifestResourceNames()) {
            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null) continue;

            if (resourceName.Contains("atlas0_whatsapp")) 
                SetEmbeddedTextureToArray(stream, WhatsappTextures, 0);
            else if (resourceName.Contains("atlas1_whatsapp"))
                SetEmbeddedTextureToArray(stream, WhatsappTextures, 1);
            else if (resourceName.Contains("atlas0_modified"))
                SetEmbeddedTextureToArray(stream, KarmelitaTextures, 0);
            else if (resourceName.Contains("atlas1_modified")) 
                SetEmbeddedTextureToArray(stream, KarmelitaTextures, 1);
            else if (resourceName.Contains("goodtimefrog"))
                GetGoodTimeFrogTexture(stream);
            else if (resourceName.Contains("WindBladeCueNoDelay"))
                GetEmbeddedClip(stream, ref WindBladeCueSound, AudioType.WAV);
            else if (resourceName.Contains("whatsapp.mp3"))
                GetEmbeddedClip(stream, ref WhatsappSound, AudioType.MPEG);
        }
        UpdateTextures();
    }

    private void SetEmbeddedTextureToArray(Stream stream, Texture2D[] textures, int index)
    {
        var buffer = new byte[stream.Length];
        stream.Read(buffer, 0, buffer.Length);
        var atlasTex = new Texture2D(2, 2);
        atlasTex.LoadImage(buffer);
        textures[index] = atlasTex;
    }

    private void GetGoodTimeFrogTexture(Stream stream)
    {
        var buffer = new byte[stream.Length];
        stream.Read(buffer, 0, buffer.Length);
        var goodtimefrogTexture = new Texture2D(2, 2);
        goodtimefrogTexture.LoadImage(buffer);
        goodTimeFrog = goodtimefrogTexture;
    }
    
    private void GetEmbeddedClip(Stream stream, ref AudioClip clipToSet, AudioType audioType)
    {
        byte[] buffer;
        using (var ms = new MemoryStream())
        {
            stream.CopyTo(ms);
            buffer = ms.ToArray();
        }

        var tempPath = Path.Combine(Application.temporaryCachePath, $"temp_mod_audio_{Guid.NewGuid()}.wav");
        File.WriteAllBytes(tempPath, buffer);

        string fileUrl = $"file:///{tempPath}"; 

        using (var uwr = UnityWebRequestMultimedia.GetAudioClip(fileUrl, audioType))
        {
            uwr.SendWebRequest();

            while (!uwr.isDone) { }

            if (uwr.result == UnityWebRequest.Result.Success)
                clipToSet = DownloadHandlerAudioClip.GetContent(uwr);
        }
        File.Delete(tempPath);
    }
    
    private void UpdateTextures() => CurrentTextures = isWhatsapp.Value ? WhatsappTextures : KarmelitaTextures; 

    public void ResetFlags()
    {
        foreach (var tracker in FindObjectsByType<HighlightTracker>(FindObjectsSortMode.None))
            tracker.ResetMaterial();
        HeroController.instance.SpriteFlash.IsBlocked = false;
        HeroController.instance.SpriteFlash.CancelFlash();
        HeroController.instance.heroLight.Alpha = 1f;
        HudGlobalHide.IsHidden = false;
        HeroController.instance.transform.Find("Vignette").gameObject.SetActive(true);
        GameObject.Find("_GameCameras/CameraParent/tk2dCamera/SceneParticlesController").SetActive(true);
    }
    
    public void OnKarmelitaSceneLoad()
    {
        ToolItemManager.TryReplenishTools(true, ToolItemManager.ReplenishMethod.BenchSilent);
        DisableBackgroundGoons();
        AddWrapper();
        SetupHeroAwake();
        AddGoodTimeFrog();
    }

    private void AddGoodTimeFrog()
    {
        GameObject frog = new GameObject(":goodtimefrog:", typeof(SpriteRenderer), typeof(BoxCollider2D), typeof(goodtimefrog));
        var position = new Vector3(61.6385f, 21.5677f, 0.010f);
        frog.transform.position = new Vector3(position.x, position.y, position.z);

        Sprite frogSprite = Sprite.Create(
            goodTimeFrog,
            new Rect(0, 0, goodTimeFrog.width, goodTimeFrog.height),
            new Vector2(0.5f, 0.5f), 
            100f                     
        );

        SpriteRenderer sr = frog.GetComponent<SpriteRenderer>();
        sr.sprite = frogSprite;

        Goodtimefrog = frog.GetComponent<goodtimefrog>();
    }
    
    private void SetupHeroAwake()
    {
        HeroController.instance.transform.position = Constants.PlayerSpawnPosition;
        HeroController.instance.FaceLeft();
    }

    private void AddWrapper()
    {
        wrapper = GameObject.Find("Boss Scene/Hunter Queen Boss").AddComponent<KarmelitaWrapper>();
        Log("Wrapper added");
    }

    private void TeleportToKarmelitaScene()
    {
        if (GameManager.instance == null || HeroController.instance == null) return;

        ResetFlags();
        
        GameManager.instance.playerData.defeatedAntQueen = false;
        if (GameManager.instance.isPaused)
        {
            StartCoroutine(GameManager.instance.PauseGameToggle(false));
        }
        var karmelitaSceneInfo = new GameManager.SceneLoadInfo()
        {
            SceneName = "Memory_Ant_Queen",
            EntryGateName = "door_wakeInMemory",
        };
        AudioManager.Instance.StopAndClearMusic();
        AudioManager.Instance.StopAndClearAtmos();
        HeroController.instance.transform.Find("crest_reaper_effects(Clone)").gameObject.SetActive(false);
        GameManager.instance.BeginSceneTransition(karmelitaSceneInfo);
    }
    
    private void DisableBackgroundGoons() => GameObject.Find("Boss Scene/Battle Scene/Wave 4").SetActive(false);

    public void Log(object message)
    {
        if (IsDebug == null) return;
        
        if (IsDebug.Value)
            Logger.LogInfo(message);
    }

    public void OnWhatsappSet(object sender, EventArgs args)
    {
        UpdateTextures();
        wrapper.KarmelitaSpriteChanger?.OverrideTextures(CurrentTextures);
        harmony?.PatchAll(typeof(LocalizationPatches));
    }
    
    public void OnDestroy()
    {
        harmony?.UnpatchSelf();
        BossButtonDrawer.OnButtonPressed -= TeleportToKarmelitaScene;
        isWhatsapp.SettingChanged -= OnWhatsappSet;
    }
}
