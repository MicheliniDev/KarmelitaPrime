using HutongGames.PlayMaker;
using UnityEngine;

namespace KarmelitaPrime;

public class SpawnPrefabAction : FsmStateAction
{
    public GameObject Prefab;
    public Vector3 Position;

    public override void OnEnter()
    {
        base.OnEnter();
        Object.Instantiate(Prefab, Position, Quaternion.identity);
    }
}