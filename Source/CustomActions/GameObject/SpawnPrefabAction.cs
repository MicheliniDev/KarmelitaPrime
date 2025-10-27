using HutongGames.PlayMaker;
using UnityEngine;

namespace KarmelitaPrime;

public class SpawnPrefabAction : FsmStateAction
{
    public GameObject Prefab;
    public Vector3 Position;
    public Transform Transform;
    public override void OnEnter()
    {
        base.OnEnter();
        var prefab = Object.Instantiate(Prefab, Transform ? Transform.position : Position, Quaternion.identity);
        if (!prefab.activeInHierarchy || !prefab.activeSelf)
            prefab.SetActive(true);
        Finish();
    }
}