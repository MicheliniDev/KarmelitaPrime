using HarmonyLib;
using UnityEngine.SceneManagement;

namespace KarmelitaPrime.Patches;

[HarmonyPatch]
public class CheckSceneTransitionPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.OnNextLevelReady))]
    private static void CheckKarmelitaScenePatch(ref GameManager __instance)
    {
        if (SceneManager.GetActiveScene().name == Constants.KarmelitaSceneName)
            KarmelitaPrimeMain.Instance.OnKarmelitaSceneLoad();
        else if (GameManager.instance.lastSceneName == Constants.KarmelitaSceneName)
            KarmelitaPrimeMain.Instance.ResetFlags();
    }
}