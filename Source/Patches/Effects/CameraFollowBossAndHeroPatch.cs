using HarmonyLib;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KarmelitaPrime.Patches.Effects;

[HarmonyPatch]
public class CameraFollowBossAndHeroPatch
{
    private static Transform heroTransform => HeroController.instance.transform;
    private static Transform karmelitaTransform => KarmelitaPrimeMain.Instance.wrapper.transform;
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(CameraTarget), nameof(CameraTarget.Update))]
    private static bool MakeCameraFollowBoss(ref CameraTarget __instance)
    {
        if (SceneManager.GetActiveScene().name == Constants.KarmelitaSceneName &&
            KarmelitaPrimeMain.Instance.wrapper)
        {
            float midpoint = (karmelitaTransform.position.x + heroTransform.position.x) / 2f;
            float midpointY = (karmelitaTransform.position.y + heroTransform.position.y) / 2f;

            Vector3 position = __instance.transform.position;
            position = new Vector3(
                Mathf.Clamp(midpoint, __instance.xLockMin, __instance.xLockMax), 
                Mathf.Clamp(midpointY,  __instance.yLockMin, __instance.yLockMax), 
                position.z);
            __instance.transform.position = position;
        
            return false;      
        }
        return true;
    }
}