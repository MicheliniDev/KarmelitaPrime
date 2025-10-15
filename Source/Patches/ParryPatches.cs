using HarmonyLib;
using UnityEngine.SceneManagement;

namespace KarmelitaPrime;

[HarmonyPatch]
public class ParryPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.NailParry))]
    private static void NailClashGiveSilkPatch(ref HeroController __instance)
    {
        __instance.AddSilk(1, true, SilkSpool.SilkAddSource.Normal, false);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.CrossStitchInvuln))]
    private static void CrossStitchGiveSilkPatch(ref HeroController __instance)
    {
        __instance.AddSilk(2, true, SilkSpool.SilkAddSource.Normal, false);
    }
}