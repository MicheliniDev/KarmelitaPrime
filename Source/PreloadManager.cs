using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HutongGames.PlayMaker.Actions;
using static KarmelitaPrime.Constants;
using UnityEngine;

namespace KarmelitaPrime;

public static class PreloadManager
{
    private static string platformPath => Application.platform switch
    {
        RuntimePlatform.WindowsPlayer => "StandaloneWindows64",
        RuntimePlatform.OSXPlayer => "StandaloneOSX",
        RuntimePlatform.LinuxPlayer => "StandaloneLinux64",
        _ => ""
    };

    private static Dictionary<string, AssetBundle> loadedBundles = new();
    public static IEnumerator Initialize()
    {
        foreach (var bundle in AssetBundle.GetAllLoadedAssetBundles()) {
            foreach (var assetPath in bundle.GetAllAssetNames())
            {
                if (!AssetNames.Any(objName => assetPath.Contains(objName))) continue;
                
                var assetLoadHandle = bundle.LoadAssetAsync(assetPath);
                yield return assetLoadHandle;

                var loadedAsset = assetLoadHandle.asset;
            }
        }
    }
}