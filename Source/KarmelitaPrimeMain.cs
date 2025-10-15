using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using TMProOld;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace KarmelitaPrime;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class KarmelitaPrimeMain : BaseUnityPlugin
{
    public static KarmelitaPrimeMain Instance;
    
    public Texture2D[] KarmelitaTextures = new Texture2D[2];
    public string Karmelita100SaveFilePath;
    public int CurrentSaveSlotNumber;
    
    private Harmony harmony;
    public KarmelitaWrapper wrapper;
    
    private int backerCredits;
    
    private ConfigEntry<bool> fightBoss;
    public ConfigEntry<bool> IsDebug;
    public ConfigEntry<bool> isWhatsapp;

    public Font MenuFont;
    
    public void Awake()
    {
        harmony = Harmony.CreateAndPatchAll(typeof(KarmelitaPrimeMain).Assembly);
        Logger.LogInfo($"{MyPluginInfo.PLUGIN_NAME} is loaded");

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
        IsDebug = Config.Bind("Settings", "IsDebug", false, "Activates certain debug functionality");
        isWhatsapp = Config.Bind("Whatsapp", "Whatsapp", false, "Whatsapp");
        
        StartCoroutine(WaitUntilGameManager());
        StartCoroutine(StoreFont());
        
        FindFont();
        
        LoadKarmelitaTextures(isWhatsapp.Value);
        
        BossButtonDrawer.OnButtonPressed += TeleportToKarmelitaScene;
        isWhatsapp.SettingChanged += OnWhatsappSet;
        
        Instance = this;
    }

    private IEnumerator WaitUntilGameManager()
    {
        yield return new WaitUntil(() => GameManager.instance);
        yield return null;
        StoreBackerCredits();
        SubscribeSceneLoadEvent();
        yield return null;
    }

    private IEnumerator StoreFont()
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Menu_Title");
        yield return null;
        FindFont();
        yield return null;
    }

    private void FindFont()
    {
        foreach (var font in Resources.FindObjectsOfTypeAll<Font>())
        {
            if (font.name.Contains("Trajan"))
            {
                MenuFont = font;
                break;
            }
        }
        Logger.LogWarning("PLEASE REMOVE THE FONT FIND ON RELEASE AND PUT IT BACK ON THE COROUTINE");
    }

    private void LoadKarmelitaTextures(bool whatsapp) {
        //Code yoinked from Jngo :P
        var assembly = Assembly.GetExecutingAssembly();
        foreach (string resourceName in assembly.GetManifestResourceNames()) {
            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null) continue;

            if (whatsapp)
            {
                if (resourceName.Contains("atlas0_whatsapp")) {
                    var buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    var atlasTex = new Texture2D(2, 2);
                    atlasTex.LoadImage(buffer);
                    KarmelitaTextures[0] = atlasTex;
                } 
                else if (resourceName.Contains("atlas1_whatsapp")) {
                    var buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    var atlasTex = new Texture2D(2, 2);
                    atlasTex.LoadImage(buffer);
                    KarmelitaTextures[1] = atlasTex;
                }
            }
            else
            {
                if (resourceName.Contains("atlas0_modified")) {
                    var buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    var atlasTex = new Texture2D(2, 2);
                    atlasTex.LoadImage(buffer);
                    KarmelitaTextures[0] = atlasTex;
                } 
                else if (resourceName.Contains("atlas1_modified")) {
                    var buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    var atlasTex = new Texture2D(2, 2);
                    atlasTex.LoadImage(buffer);
                    KarmelitaTextures[1] = atlasTex;
                }
            }
        }
    }
    
    public void CheckKarmelitaSceneLoad()
    {
        GameManager.instance.gameSettings.backerCredits = backerCredits;
        if (SceneManager.GetActiveScene().name == Constants.KarmelitaSceneName)
        {
            Log("Karmelita scene loaded");
            OnKarmelitaSceneLoad();
        }
    }
    
    public void OnKarmelitaSceneLoad()
    {
        DisableBackgroundGoons();
        AddWrapper();
        ForceCredits();
        SetupHeroAwake();
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
        GameManager.instance.BeginSceneTransition(karmelitaSceneInfo);
    }
    
    private void SubscribeSceneLoadEvent() => GameManager.instance.OnFinishedSceneTransition += CheckKarmelitaSceneLoad;
    private void StoreBackerCredits() => backerCredits = GameManager.instance.gameSettings.backerCredits;
    private void ForceCredits() => GameManager.instance.gameSettings.backerCredits = 1;
    private void DisableBackgroundGoons() => GameObject.Find("Boss Scene/Battle Scene/Wave 4").SetActive(false);

    public void Log(string message)
    {
        if (IsDebug.Value)
            Logger.LogInfo(message);
    }

    public void OnWhatsappSet(object sender, EventArgs args)
    {
        LoadKarmelitaTextures(isWhatsapp.Value);
        wrapper?.ChangeTextures();
        harmony?.PatchAll(typeof(LocalizationPatches));
    }
    
    public void OnDestroy()
    {
        harmony?.UnpatchSelf();
        GameManager.instance.OnFinishedSceneTransition -= CheckKarmelitaSceneLoad;
        BossButtonDrawer.OnButtonPressed -= TeleportToKarmelitaScene;
        isWhatsapp.SettingChanged -= OnWhatsappSet;
    }
}
