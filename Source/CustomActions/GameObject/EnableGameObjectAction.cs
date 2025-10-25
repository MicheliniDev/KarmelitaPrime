using HutongGames.PlayMaker;
using UnityEngine;

namespace KarmelitaPrime;

public class EnableGameObjectAction : FsmStateAction
{
    public GameObject GameObject;
    public bool Enable;
    public bool ResetOnExit;
    public override void OnEnter()
    {
        base.OnEnter();
        GameObject.SetActive(Enable);
        Finish();
    }

    public override void OnExit()
    {
        if (ResetOnExit)
            GameObject.SetActive(false);
    }
}