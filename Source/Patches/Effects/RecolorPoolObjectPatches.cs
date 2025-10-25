using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace KarmelitaPrime;

[HarmonyPatch]
public class RecolorPoolObjectPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(SpawnObjectFromGlobalPool), nameof(SpawnObjectFromGlobalPool.OnEnter))]
    private static void RecolorPoolObjects(ref SpawnObjectFromGlobalPool __instance)
    {
        //For some reason checking the UI layer isn't working so I gotta write all exceptions :D
        //AND IT STILL DOESN'T WORK??????????????????????????
        if (!Constants.IsBlackWhiteHighlight 
            || __instance.storeObject.Value.layer == LayerMask.NameToLayer("UI")
            || __instance.storeObject.Value.name.Contains("health")
            || __instance.storeObject.Value.name.Contains("Silk Chunk")) return;

        if (__instance.storeObject.Value.name.Contains("Sickle"))
        {
            HandleKarmelitaSickle(__instance.storeObject.Value);
            return;
        }
        
        if (!__instance.storeObject.Value.TryGetComponent<HighlightTracker>(out var highlightTracker))
            highlightTracker = __instance.storeObject.Value.AddComponent<HighlightTracker>();
        highlightTracker.ApplyHighlightEffect();
    }
    
    private static void HandleKarmelitaSickle(GameObject sickle)
    {
        if (!sickle.TryGetComponent<HighlightTracker>(out var tracker))
            tracker = sickle.AddComponent<HighlightTracker>();
        tracker.IsSickle = true;
        tracker.ApplyHighlightEffect();
        
        var renderer = sickle.TryGetComponent<SpriteFlash>(out var flasher)
            ? flasher
            : sickle.AddComponent<SpriteFlash>();
        renderer.Flash(Color.white, 1f, 0f, 9999f, 0f);
        foreach (var meshRenderer in sickle.GetComponentsInChildren<MeshRenderer>(true))
        {
            var childSpriteFlasher = meshRenderer.TryGetComponent<SpriteFlash>(out var childFlasher)
                ? childFlasher
                : meshRenderer.gameObject.AddComponent<SpriteFlash>();
            childSpriteFlasher.Flash(Color.white, 1f, 0f, 9999f, 0f);
        }
    }
}