using System.Collections;
using HarmonyLib;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KarmelitaPrime;

[HarmonyPatch]
public class BossTitlePatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(DisplayBossTitle), nameof(DisplayBossTitle.OnEnter))]
    private static void MakeBossTitleBigPatch(ref DisplayBossTitle __instance)
    {
        if (SceneManager.GetActiveScene().name != Constants.KarmelitaSceneName) return;
        KarmelitaPrimeMain.Instance.StartCoroutine(BossTitleRoutine(__instance));
        //I can't believe this worked
    }

    private static IEnumerator BossTitleRoutine(DisplayBossTitle __instance)
    {
        GameObject gameObject = ManagerSingleton<AreaTitle>.Instance.gameObject;
        PlayMakerFSM gameObjectFsm = ActionHelpers.GetGameObjectFsm(gameObject, "Area Title Control");
        __instance.areaTitleObject.Value = gameObject;
        
        gameObject.SetActive(false);
        gameObjectFsm.FsmVariables.FindFsmBool("Visited").Value = false;
        gameObjectFsm.FsmVariables.FindFsmBool("Display Right").Value = __instance.displayRight.Value;
        gameObjectFsm.FsmVariables.FindFsmString("Area Event").Value = __instance.bossTitle.Value;
        gameObjectFsm.FsmVariables.FindFsmBool("NPC Title").Value = false;
        gameObject.SetActive(true);
        
        var mainAnimator = gameObject.GetComponentInChildren<Animator>();
        mainAnimator.speed = 1.15f;
        mainAnimator.GetComponentInChildren<Animator>().speed = 1.15f;
        yield return new WaitForSeconds(3f);
        var fsm = gameObject.GetComponent<PlayMakerFSM>();
        fsm.SendEvent("FINISHED");
    }
}