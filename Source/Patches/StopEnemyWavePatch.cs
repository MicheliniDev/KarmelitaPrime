using HarmonyLib;
using UnityEngine.SceneManagement;

namespace KarmelitaPrime.Patches;

[HarmonyPatch]
public class StopEnemyWavePatch
{    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(BattleScene), nameof(BattleScene.StartBattle))]
    private static void StopEnemyWave(ref BattleScene __instance)
    {
        if (SceneManager.GetActiveScene().name != Constants.KarmelitaSceneName) return;
        __instance.endScene.SendEvent("BATTLE END");
        __instance.gameObject.SetActive(false);
    }
}