using System.Linq;
using UnityEngine;

namespace KarmelitaPrime;

public class HighlightTracker : MonoBehaviour
{
    private Renderer[] renderers;
    private Material[] originalMaterials;
    private Color[] originalColors;
    private ParticleSystem.MinMaxGradient[] originalGradients;
    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>(true);
        if (TryGetComponent<Renderer>(out var mainRenderer))
            renderers = renderers.Append(mainRenderer).ToArray();
        
        originalMaterials = new Material[renderers.Length];
        originalColors = new Color[renderers.Length];
        originalGradients = new ParticleSystem.MinMaxGradient[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            var renderer = renderers[i];
            originalMaterials[i] = renderer.sharedMaterial;

            if (renderer.sharedMaterial != null && renderer.sharedMaterial.HasProperty("_Color"))
                originalColors[i] = renderer.sharedMaterial.color;

            if (renderer is ParticleSystemRenderer psRenderer)
            {
                var ps = psRenderer.GetComponent<ParticleSystem>();
                var colLifetime = ps.colorOverLifetime;
                originalGradients[i] = colLifetime.color;
            }
        }
    }

    public void OnDisable()
    {
        ResetMaterial();
    }
    
    public void ApplyHighlightEffect()
    {
        var flashShader = KarmelitaPrimeMain.Instance.FlashShader;

        foreach (var renderer in renderers)
        {
            if (renderer is ParticleSystemRenderer psRenderer)
            {
                var ps = psRenderer.GetComponent<ParticleSystem>();
                var colLifetime = ps.colorOverLifetime;
                colLifetime.color = Color.white;
            }
            else if (renderer is MeshRenderer or SpriteRenderer)
            {
                var mat = new Material(flashShader);
                mat.SetFloat(Shader.PropertyToID("_FlashAmount"), 1f);
                mat.SetColor(Shader.PropertyToID("_FlashColor"), Color.white);
                renderer.material = mat; 
            }
        }
    }

    public void ResetMaterial()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            var renderer = renderers[i];
            renderer.sharedMaterial = originalMaterials[i];
            renderer.sharedMaterial.color = originalColors[i];

            if (renderer is ParticleSystemRenderer psRenderer)
            {
                var ps = psRenderer.GetComponent<ParticleSystem>();
                var colLifetime = ps.colorOverLifetime;
                colLifetime.color = originalGradients[i];
            }
        }
    }
}