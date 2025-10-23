using HutongGames.PlayMaker;
using UnityEngine;

namespace KarmelitaPrime;

public class CheckHeroTooCloseAction : FsmStateAction
{
    public GameObject Owner;
    public float Threshold;
    public FsmEvent TrueEvent;
    public FsmEvent FalseEvent;

    public override void OnEnter()
    {
        if (GetDistance() < Threshold && TrueEvent != null)
            Fsm.Event(TrueEvent);
        else if (FalseEvent != null)
            Fsm.Event(FalseEvent);
    }

    private float GetDistance() => Mathf.Abs(HeroController.instance.transform.position.x - Owner.transform.position.x);
}