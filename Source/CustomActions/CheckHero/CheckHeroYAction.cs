using HutongGames.PlayMaker;
using UnityEngine;

namespace KarmelitaPrime;

public class CheckHeroYAction : FsmStateAction
{
    public Transform Target;
    
    public FsmEvent AboveEvent;
    public FsmEvent BelowEvent;
    
    private bool hasSentEvent;
    public override void OnEnter()
    {
        base.OnEnter();
        Fsm.Event(HeroController.instance.transform.position.y > Target.position.y ? AboveEvent : BelowEvent);
    }
}