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
    [HarmonyPrefix]
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.TakeDamage))]
    private static void CancelContactDamageOnKarmelita(ref HeroController __instance, ref GameObject go, ref int damageAmount)
    {
        if (SceneManager.GetActiveScene().name != Constants.KarmelitaSceneName || 
            !go || !__instance || !KarmelitaPrimeMain.Instance.wrapper)
        {
            return;
        }

        if (go.layer == 11 && !KarmelitaPrimeMain.Instance.wrapper.ShouldDealContactDamage())
        {
            damageAmount = 0;
            __instance.AnimCtrl.StartControl();
            __instance.CancelAttack();
            __instance.RegainControl();
            __instance.StartAnimationControl();
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