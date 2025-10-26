using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KarmelitaPrime;

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
}