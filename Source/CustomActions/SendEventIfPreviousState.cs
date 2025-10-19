using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class SendEventIfPreviousState : FsmStateAction
{
    public PlayMakerFSM fsm;
    public string StateName;
    public FsmEvent trueEvent;
    public FsmEvent falseEvent;
    
    public override void OnEnter()
    {
        base.OnEnter();
        fsm.Fsm.Event(fsm.Fsm.PreviousActiveState.Name == StateName ? trueEvent : falseEvent);
    }
}