using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace KarmelitaPrime;

public static class PreloadManager
{
    
    
    public static async Task<GameObject> PreloadObject(string assetBundlePath, string sceneName, 
        string objectName, 
        Action<GameObject> onComplete)
    {
        AssetBundleCreateRequest assetBundleLoadRequest = AssetBundle.LoadFromFileAsync(assetBundlePath);
        await assetBundleLoadRequest;
        
        await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (!scene.IsValid())
        {
            KarmelitaPrimeMain.Instance.Log($"Failed to load scene: {sceneName}");
            onComplete?.Invoke(null);
            return null;
        }

        GameObject result = null;
        GameObject[] rootGameObjects = scene.GetRootGameObjects();
        foreach (GameObject go in rootGameObjects)
        {
            result = FindInChildren(go, objectName);
            if (result != null)
                break;
        }

        if (result != null)
        {
            GameObject instance = Object.Instantiate(result, null);
            result = instance;
            KarmelitaPrimeMain.Instance.Log($"Successfully preloaded {result.name}");
        }
        else
        {
            KarmelitaPrimeMain.Instance.Log($"Warning: Could not find GameObject named '{objectName}' in scene '{sceneName}'.");
        }

        await SceneManager.UnloadSceneAsync(sceneName);

        onComplete?.Invoke(result);
        return result;
    }

    private static GameObject FindInChildren(GameObject parent, string targetName)
    {
        if (parent.name == targetName)
            return parent;

        foreach (Transform child in parent.transform)
        {
            GameObject found = FindInChildren(child.gameObject, targetName);
            if (found != null)
                return found;
        }
        return null;
    }
}