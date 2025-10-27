using System.Collections;
using GlobalEnums;
using HarmonyLib;
using HutongGames.PlayMaker.Actions;
using TeamCherry.Localization;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KarmelitaPrime.Patches;

[HarmonyPatch]
public class DamagePatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(HealthManager), "TakeDamage")]
    private static void AutoPhase3TriggerPatch(ref HealthManager __instance, ref HitInstance hitInstance)
    {
        if (SceneManager.GetActiveScene().name != Constants.KarmelitaSceneName
            || !KarmelitaPrimeMain.Instance.wrapper) return; 
        
        if (__instance.hp <= Constants.KarmelitaPhase3HpThreshold)
            KarmelitaPrimeMain.Instance.wrapper.TriggerPhase3();
        else if (__instance.hp <= Constants.KarmelitaPhase2_5HpThreshold)
            KarmelitaPrimeMain.Instance.wrapper.FakeP3();
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.TakeDamage))]
    private static bool CancelContactDamageOnKarmelita(ref HeroController __instance, ref GameObject go, ref int damageAmount)
    {
        if (KarmelitaPrimeMain.Instance.wrapper &&
            !KarmelitaPrimeMain.Instance.wrapper.ShouldDealContactDamage()
            && go.name == "Hunter Queen Boss")
        {
            return false;
        }
        return true;
    }
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.TakeDamage))]
    private static void BoostDamageFromSource(ref HeroController __instance, ref GameObject go, ref int damageAmount)
    {
        if (go.name.Contains("Song Knight Projectile"))
        {
            damageAmount = 3;
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