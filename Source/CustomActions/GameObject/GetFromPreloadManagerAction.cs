using HutongGames.PlayMaker;
using UnityEngine;

namespace KarmelitaPrime;

public class GetFromPreloadManagerAction : FsmStateAction
{
    public string PrefabName;
    public Transform SpawnPosition;
    public override void OnEnter()
    {
        base.OnEnter();
        PreloadManager.Get<GameObject>(PrefabName, (prefab) =>
        {
            KarmelitaPrimeMain.Instance.wrapper.OnPrefabSpawn(prefab, SpawnPosition);
        });
        Finish();
    }
}