using HutongGames.PlayMaker;
using UnityEngine;

namespace KarmelitaPrime;

public class CheckHeroYAction : FsmStateAction
{
    public Transform Target;

    public float Threshold;
    public FsmEvent AboveEvent;
    public FsmEvent BelowEvent;
    public override void OnEnter()
    {
        base.OnEnter();
        if (HeroController.instance.transform.position.y > (Target.position.y + Threshold))
            Fsm.Event(AboveEvent);
        else 
            Fsm.Event(BelowEvent);
        Finish();
    }
}