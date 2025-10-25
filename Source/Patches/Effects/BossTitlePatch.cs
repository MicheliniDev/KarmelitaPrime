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
        KarmelitaPrimeMain.Instance.StartCoroutine(BossTitleRoutine());
        //I can't believe this worked
    }

    private static IEnumerator BossTitleRoutine()
    {
        GameObject gameObject = ManagerSingleton<AreaTitle>.Instance.gameObject;
        gameObject.SetActive(false);
        gameObject.SetActive(true);
        var mainAnimator = gameObject.GetComponentInChildren<Animator>();
        mainAnimator.speed = 1.15f;
        mainAnimator.GetComponentInChildren<Animator>().speed = 1.15f;
        yield return new WaitForSeconds(3f);
        var fsm = gameObject.GetComponent<PlayMakerFSM>();
        fsm.SendEvent("FINISHED");
    }
}