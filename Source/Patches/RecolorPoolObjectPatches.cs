using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace KarmelitaPrime;

[HarmonyPatch]
public class RecolorPoolObjectPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(SpawnObjectFromGlobalPool), nameof(SpawnObjectFromGlobalPool.OnEnter))]
    private static void RecolorPoolObjects(ref SpawnObjectFromGlobalPool __instance)
    {
        if (!Constants.IsBlackWhiteHighlight || __instance.storeObject.Value.layer == 5) return;

        if (__instance.storeObject.Value.name.Contains("Sickle"))
        {
            HandleKarmelitaSickle(__instance.storeObject.Value);
            return;
        }
        HandleGeneralRecolor(__instance.storeObject.Value);
    }

    private static void HandleKarmelitaSickle(GameObject sickle)
    {
        var renderer = sickle.TryGetComponent<SpriteFlash>(out var flasher)
            ? flasher
            : sickle.AddComponent<SpriteFlash>();
        renderer.Flash(Color.white, 1f, 0f, 9999f, 0f);
        foreach (var meshRenderer in sickle.GetComponentsInChildren<MeshRenderer>(true))
        {
            var childSpriteFlasher = meshRenderer.TryGetComponent<SpriteFlash>(out var childFlasher)
                ? childFlasher
                : meshRenderer.gameObject.AddComponent<SpriteFlash>();
            childSpriteFlasher.Flash(Color.white, 1f, 0f, 9999f, 0f);
        }
    }
    
    private static void HandleGeneralRecolor(GameObject storeObject)
    {
        var flashShader = KarmelitaPrimeMain.Instance.FlashShader;
        
        var renderers = storeObject.GetComponentsInChildren<Renderer>(true);
        if (storeObject.TryGetComponent<Renderer>(out var mainRenderer))
            renderers = renderers.Append(mainRenderer).ToArray();
        
        foreach (var renderer in renderers)
        {
            if (renderer is ParticleSystemRenderer)
            {
                var particleSystem = renderer.GetComponent<ParticleSystem>();
                var colorLifetime = particleSystem.colorOverLifetime;
                colorLifetime.color = Color.white;
                continue;
            }

            if (renderer is MeshRenderer or SpriteRenderer)
            {
                renderer.material = new Material(flashShader);
                var flash = renderer.TryGetComponent<SpriteFlash>(out var flasher) 
                    ? flasher 
                    : renderer.gameObject.AddComponent<SpriteFlash>();
                flash.Flash(Color.white, 1f, 0f, 999f, 0f);
            }
        }
    }
}