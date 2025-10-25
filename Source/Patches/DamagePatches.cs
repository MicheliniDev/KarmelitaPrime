using System.Collections;
using GlobalEnums;
using HarmonyLib;
using HutongGames.PlayMaker.Actions;
using TeamCherry.Localization;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KarmelitaPrime;

[HarmonyPatch]
public class DamagePatches
{
    /*[HarmonyPrefix]
    [HarmonyPatch(typeof(HeroController), "StartInvulnerable")]
    private static void AlterIFrameDuration(ref HeroController __instance, ref float duration)
    {
        duration /= 2f;
    }*/
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(HealthManager), "TakeDamage")]
    private static void ClawlineAuraPatch(ref HealthManager __instance, ref HitInstance hitInstance)
    {
        if (SceneManager.GetActiveScene().name != Constants.KarmelitaSceneName
            || !KarmelitaPrimeMain.Instance.wrapper) return;

        if (__instance.hp <= Constants.KarmelitaPhase3HpThreshold)
            KarmelitaPrimeMain.Instance.wrapper.TriggerPhase3();
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.TakeDamage))]
    private static void CancelContactDamageOnKarmelita(ref HeroController __instance, ref GameObject go, ref int damageAmount)
    {
        if (SceneManager.GetActiveScene().name == Constants.KarmelitaSceneName &&
            go.name == "Hunter Queen Boss" && go.layer == 11 && 
            KarmelitaPrimeMain.Instance.wrapper &&
            !KarmelitaPrimeMain.Instance.wrapper.ShouldDealContactDamage())
        {
            damageAmount = 0;
            return;
        }
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.TakeDamage))]
    private static void AutoHealOnDamageDebugPatch(ref HeroController __instance)
    {
        if (KarmelitaPrimeMain.Instance.IsDebug.Value)
            __instance.MaxHealth();
    }
}