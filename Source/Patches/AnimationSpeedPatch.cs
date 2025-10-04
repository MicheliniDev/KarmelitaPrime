using HarmonyLib;
using UnityEngine.SceneManagement;

namespace KarmelitaPrime;

[HarmonyPatch]
public class AnimationSpeedPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(tk2dSpriteAnimator), nameof(tk2dSpriteAnimator.Play),
        [typeof(tk2dSpriteAnimationClip), typeof(float), typeof(float)])]
    private static void OverrideKarmelitaFpsPatch(ref tk2dSpriteAnimator __instance, ref tk2dSpriteAnimationClip clip,
        ref float clipStartTime, ref float overrideFps)
    {
        if (KarmelitaPrimeMain.Instance && KarmelitaPrimeMain.Instance.wrapper && SceneManager.GetActiveScene().name == Constants.KarmelitaSceneName &&
            __instance.gameObject.name == "Hunter Queen Boss")
        {
            overrideFps = clip.fps * KarmelitaPrimeMain.Instance.wrapper.GetAnimationSpeedModifier(clip.name);
        }
    }
}