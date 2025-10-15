using HarmonyLib;
using UnityEngine.SceneManagement;

namespace KarmelitaPrime;

[HarmonyPatch]
public class AuraFarmPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(HealthManager), "TakeDamage")]
    private static void ClawlineAuraPatch(ref HealthManager __instance, ref HitInstance hitInstance)
    {
        if (SceneManager.GetActiveScene().name != Constants.KarmelitaSceneName
            || !KarmelitaPrimeMain.Instance.wrapper || 
            hitInstance.Source.name != "Harpoon Dash Damager") return;

        KarmelitaPrimeMain.Instance.wrapper.FarmAura(20f);
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(tk2dSpriteAnimator), nameof(tk2dSpriteAnimator.Play),
        [typeof(tk2dSpriteAnimationClip), typeof(float), typeof(float)])]
    private static void CheckPlayerAuraFarmPatch(ref tk2dSpriteAnimator __instance, ref tk2dSpriteAnimationClip clip,
        ref float clipStartTime, ref float overrideFps)
    {
        if (!__instance.gameObject.name.Contains("Hero") ||
            SceneManager.GetActiveScene().name != Constants.KarmelitaSceneName) return;

        switch (clip.name)
        {
            case "Sprint Backflip":
                KarmelitaPrimeMain.Instance.wrapper.FarmAura(20);
                break;
            case "Taunt":
                KarmelitaPrimeMain.Instance.wrapper.FarmAura(50f, true);
                break;
            case "DownSpikeBounce":
                KarmelitaPrimeMain.Instance.wrapper.FarmAura(7f, true);
                break;
        }
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.TakeDamage))]
    private static void LoseAuraPatch(ref HeroController __instance)
    {
        if (SceneManager.GetActiveScene().name != Constants.KarmelitaSceneName
            || !__instance || !KarmelitaPrimeMain.Instance.wrapper)
            return;
        
        KarmelitaPrimeMain.Instance.wrapper.LoseAura(15f);
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.NailParry))]
    private static void NailClashAuraPatch(ref HeroController __instance)
    {
        if (SceneManager.GetActiveScene().name == Constants.KarmelitaSceneName)
        {
            KarmelitaPrimeMain.Instance.wrapper.FarmAura(5f);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.CrossStitchInvuln))]
    private static void CrossStitchAuraPatch(ref HeroController __instance)
    {
        if (SceneManager.GetActiveScene().name == Constants.KarmelitaSceneName)
        {
            KarmelitaPrimeMain.Instance.wrapper.FarmAura(10f);
        }
    }
}