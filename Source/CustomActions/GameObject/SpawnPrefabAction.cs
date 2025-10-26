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
        Object.Instantiate(Prefab, Transform ? Transform.position : Position, Quaternion.identity);
    }
}