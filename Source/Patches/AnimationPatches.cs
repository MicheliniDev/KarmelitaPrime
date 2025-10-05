using HarmonyLib;
using HutongGames.PlayMaker.Actions;
using UnityEngine.SceneManagement;

namespace KarmelitaPrime;

[HarmonyPatch]
public class AnimationPatch
{
    private static KarmelitaWrapper wrapper => KarmelitaPrimeMain.Instance.wrapper;
    
    [HarmonyPrefix]
    [HarmonyPatch(typeof(tk2dSpriteAnimator), nameof(tk2dSpriteAnimator.Play),
        [typeof(tk2dSpriteAnimationClip), typeof(float), typeof(float)])]
    private static void OverrideAnimationStatsPatch(ref tk2dSpriteAnimator __instance, ref tk2dSpriteAnimationClip clip,
        ref float clipStartTime, ref float overrideFps)
    {
        if (KarmelitaPrimeMain.Instance && wrapper && SceneManager.GetActiveScene().name == Constants.KarmelitaSceneName &&
            __instance.gameObject.name == "Hunter Queen Boss")
        {
            clipStartTime = wrapper.GetAnimationStartTime();
            overrideFps = clip.fps * wrapper.GetAnimationSpeedModifier(clip.name);
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(tk2dSpriteAnimator), nameof(tk2dSpriteAnimator.IsPlaying), [typeof(tk2dSpriteAnimationClip)])]
    private static void AllowSameAnimationPlayPatch(ref tk2dSpriteAnimator __instance, ref bool __result)
    {
        if (SceneManager.GetActiveScene().name == Constants.KarmelitaSceneName && __instance.gameObject.name == "Hunter Queen Boss")
            __result = false;
    }
}