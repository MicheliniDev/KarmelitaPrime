using System.Collections;
using HutongGames.PlayMaker;
using KarmelitaPrime.Managers;
using UnityEngine;

namespace KarmelitaPrime;

public class GetFromPreloadManagerAction : FsmStateAction
{
    public string PrefabName;
    public Transform SpawnPosition;
    public float GetDelay;
    public override void OnEnter()
    {
        base.OnEnter();
        Fsm.Owner.StartCoroutine(WaitInstantiate(GetDelay));
    }

    private IEnumerator WaitInstantiate(float duration)
    {
        yield return new WaitForSeconds(duration);
        PreloadManager.Get<GameObject>(PrefabName, (prefab) =>
        {
            KarmelitaPrimeMain.Instance.wrapper.OnPrefabSpawn(prefab, SpawnPosition);
        });
        Finish();
    }
}