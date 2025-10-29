using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KarmelitaPrime.Patches.Effects;

[HarmonyPatch]
public class SpriteFlashPatch
{
    private static bool ShouldBlock()
    {
        return Constants.IsBlackWhiteHighlight;
    }   

    [HarmonyPrefix]
    [HarmonyPatch(typeof(SpriteFlash),"SetParams")]
    private static void StopFlashRoutine(ref SpriteFlash __instance, ref float flashAmount, ref Color flashColour)
    {
        if (ShouldBlock() && __instance)
        {
            flashAmount = 1f;
            flashColour = Color.white;
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(SpriteFlash), "Awake")]
    private static void InitializeListsPatch(SpriteFlash __instance)
    {
        var parentsField = AccessTools.Field(typeof(SpriteFlash), "parents");
        var childrenField = AccessTools.Field(typeof(SpriteFlash), "children");

        if (parentsField.GetValue(__instance) == null)
            parentsField.SetValue(__instance, new List<SpriteFlash>());

        if (childrenField.GetValue(__instance) == null)
            childrenField.SetValue(__instance, new List<SpriteFlash>());
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(SpriteFlash), "SetParamsChildrenRecursive")]
    private static bool PreventNullReferencePatch(SpriteFlash __instance)
    {
        if (Constants.IsBlackWhiteHighlight)
        { 
            return false; 
        }
        return true; 
    }
}