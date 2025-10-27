using HutongGames.PlayMaker;
using UnityEngine;

namespace KarmelitaPrime;

public class CheckHeroTooCloseAction : FsmStateAction
{
    public GameObject Owner;
    public float Threshold;
    public bool IsTooFarCheck;
    public FsmEvent TrueEvent;
    public FsmEvent FalseEvent;

    public override void OnEnter()
    {
        bool isTrue = IsTooFarCheck ? GetDistance() > Threshold : GetDistance() < Threshold;
        
        if (isTrue && TrueEvent != null)
        {
            Fsm.Event(TrueEvent);
        }
        else if (FalseEvent != null)
            Fsm.Event(FalseEvent);
        Finish();
    }

    private float GetDistance() => Mathf.Abs(HeroController.instance.transform.position.x - Owner.transform.position.x);
}