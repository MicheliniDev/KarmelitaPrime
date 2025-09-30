using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KarmelitaPrime;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class KarmelitaPrimeMain : BaseUnityPlugin
{
    public static KarmelitaPrimeMain Instance;
    
    public Texture2D[] KarmelitaTextures = new Texture2D[2];
    
    private Harmony harmony;
    public KarmelitaWrapper wrapper;
    
    private int backerCredits;
    
    public ConfigEntry<bool> IsDebug;
    public ConfigEntry<bool> isWhatsapp;
    public void Awake()
    {
        harmony = Harmony.CreateAndPatchAll(typeof(KarmelitaPrimeMain).Assembly);
        Logger.LogInfo($"{MyPluginInfo.PLUGIN_NAME} is loaded");

        IsDebug = Config.Bind("General", "IsDebug", false, "Activates certain debug functionality");
        isWhatsapp = Config.Bind("Whatsapp", "Whatsapp", false, "Whatsapp");
        
        StartCoroutine(WaitUntilGameManager());
        LoadKarmelitaTextures(isWhatsapp.Value);
        
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
        AdjustHeroPosition();
    }

    private void AdjustHeroPosition()
    {
        HeroController.instance.transform.position = Constants.PlayerSpawnPosition;
        HeroController.instance.FaceLeft();
    }
    
    private void AddWrapper()
    {
        GameObject bossGo = GameObject.Find("Boss Scene/Hunter Queen Boss");
        wrapper = bossGo.AddComponent<KarmelitaWrapper>();
        Log("Wrapper added");
    }

    private void SubscribeSceneLoadEvent() => GameManager.instance.OnFinishedSceneTransition += CheckKarmelitaSceneLoad;
    private void StoreBackerCredits() => backerCredits = GameManager.instance.gameSettings.backerCredits;
    private void ForceCredits() => GameManager.instance.gameSettings.backerCredits = 1;
    private void DisableBackgroundGoons() => GameObject.Find("Boss Scene/Battle Scene/Wave 4").SetActive(false);
    public void Log(string message) => Logger.LogInfo(message);

    public void OnWhatsappSet(object sender, EventArgs args)
    {
        switch (isWhatsapp.Value)
        {
            case true:
                LoadKarmelitaTextures(true);
                break;
            case false:
                LoadKarmelitaTextures(false);
                break;
        }
        wrapper?.ChangeTextures();
        harmony?.PatchAll(typeof(LocalizationPatches));
    }
    
    public void OnDestroy()
    {
        harmony?.UnpatchSelf();
        GameManager.instance.OnFinishedSceneTransition -= CheckKarmelitaSceneLoad;
        isWhatsapp.SettingChanged -= OnWhatsappSet;
    }
}
