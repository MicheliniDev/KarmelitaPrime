using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;
using static KarmelitaPrime.Constants;

namespace KarmelitaPrime.Managers;

public static class PreloadManager
{
    private static Dictionary<string, Object> preloadedAssets = new();
    private static Dictionary<string, AssetBundle> loadedBundles = new();
    
    private static readonly string platformPath = Application.platform switch
    {
        RuntimePlatform.WindowsPlayer => "StandaloneWindows64",
        RuntimePlatform.OSXPlayer => "StandaloneOSX",
        RuntimePlatform.LinuxPlayer => "StandaloneLinux64",
        _ => ""
    };

    public static bool AssetsLoaded;
    public static IEnumerator LoadAllAssets()
    {
        AssetsLoaded = false;
        foreach (string bundleName in BundleNames)
        {
            var bundle = AssetBundle.GetAllLoadedAssetBundles().FirstOrDefault(bundle => bundle.name == bundleName);
            if (bundle)
            {
                KarmelitaPrimeMain.Instance.Log($"PreloadManager: {bundle.name} WAS ALREADY LOADED");
                loadedBundles.TryAdd(bundle.name, bundle);
                yield return TryLoadAssets(bundle);
            }
            else
            {
                string bundlePath = $"{Addressables.RuntimePath}/{platformPath}/{bundleName}.bundle";
                var bundleLoadRequest = AssetBundle.LoadFromFileAsync(bundlePath);
                yield return bundleLoadRequest;

                var loadedBundle = bundleLoadRequest.assetBundle;
                if (!loadedBundle)
                {
                    KarmelitaPrimeMain.Instance.Log($"WHAT THE FUCK DO YOU MEAN THE BUNDLE IS NULL????????W");
                    continue; 
                }
                KarmelitaPrimeMain.Instance.Log($"PreloadManager: Successfully loaded bundle: {loadedBundle.name}");
                loadedBundles.Add(loadedBundle.name, loadedBundle);
                yield return TryLoadAssets(loadedBundle); 
            }
        }
        KarmelitaPrimeMain.Instance.Log("PreloadManager: Finished loading all specified assets.");
        AssetsLoaded = true;
    }

    private static IEnumerator TryLoadAssets(AssetBundle assetBundle)
    {
        foreach (string assetPath in assetBundle.GetAllAssetNames())
        {
            foreach (string assetName in AssetNames)
            {
                if (assetPath.Contains(assetName))
                {
                    KarmelitaPrimeMain.Instance.Log(assetPath);
                    
                    var assetLoadRequest = assetBundle.LoadAssetAsync(assetPath);
                    yield return assetLoadRequest;
                    
                    var loadedAsset = assetLoadRequest.asset;
                    if (loadedAsset)
                    {
                        KarmelitaPrimeMain.Instance.Log(preloadedAssets.TryAdd(loadedAsset.name, loadedAsset)
                            ? $"Preloaded Asset: {loadedAsset.name}"
                            : $"Asset named '{loadedAsset.name}' already exists in the dictionary. Skipping.");
                    }
                    else
                        KarmelitaPrimeMain.Instance.Log($"FAILED to load asset at path: {assetPath}");
                }
            }
        } 
    }
    
    public static T Get<T>(string name, Action<T> callback = null) where T : Object
    {
        if (preloadedAssets.TryGetValue(name, out Object asset))
        {
            T typedAsset = asset as T;
            if (!typedAsset)
            {
                KarmelitaPrimeMain.Instance.Log($"PreloadManager: Asset '{name}' was found, but it is not of the requested type '{typeof(T).Name}'. It is a '{asset.GetType().Name}'.");
            }
            callback?.Invoke(typedAsset);
            return typedAsset;
        }
        KarmelitaPrimeMain.Instance.Log($"PreloadManager: Asset '{name}' not found in preloaded assets.");
        return null;
    }

    public static void UnloadAll()
    {
        foreach (var bundle in loadedBundles)
        {
            bundle.Value.Unload(true);
            KarmelitaPrimeMain.Instance.Log($"Unloaded and destroyed assets from bundle: {bundle.Key}");
        }
        preloadedAssets.Clear();
        loadedBundles.Clear();
    }
}