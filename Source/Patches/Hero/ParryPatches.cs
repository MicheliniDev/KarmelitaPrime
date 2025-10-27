using HarmonyLib;
using UnityEngine.SceneManagement;

namespace KarmelitaPrime.Patches.Hero;

[HarmonyPatch]
public class ParryPatches
{
    /*[HarmonyPostfix]
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.CrossStitchInvuln))]
    private static void CrossStitchGiveSilkPatch(ref HeroController __instance)
    {
        __instance.AddSilk(2, true, SilkSpool.SilkAddSource.Normal, false);
    }*/
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.NailParry))]
    private static void NailClashGiveSilkPatch(ref HeroController __instance)
    {
        __instance.AddSilk(1, true, SilkSpool.SilkAddSource.Normal, false);
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.NailParry))]
    private static void NailClashDealDamagePatch(ref HeroController __instance)
    {
        if (SceneManager.GetActiveScene().name == Constants.KarmelitaSceneName
            && KarmelitaPrimeMain.Instance.wrapper)
        {
            KarmelitaPrimeMain.Instance.wrapper.health.hp -= 15;
        }
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(DamageHero), "NailClash")]
    private static void NailClashCancelProjectilePatch(ref DamageHero __instance)
    {
        if (__instance.gameObject.name.Contains("Song Knight Projectile"))
            __instance.enabled = false;
    }
}