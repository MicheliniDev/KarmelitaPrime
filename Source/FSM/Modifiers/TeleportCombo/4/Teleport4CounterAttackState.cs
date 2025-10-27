using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class Teleport4CounterAttackState(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Teleport 4 Counter Attack";
    public override void OnCreateModifier()
    {
        FsmState bindState = new FsmState(fsm.Fsm)
        {
            Name = BindState,
            Transitions = [
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                    ToState = "Teleport 4 Wind Slash 1",
                    ToFsmState = fsm.Fsm.GetState("Teleport 4 Wind Slash 1")
                }
            ]
        };
        fsm.Fsm.States = fsm.Fsm.States.Append(bindState).ToArray();
        fsmController.CloneActions(fsm.Fsm.GetState("Counter Attack"), BindFsmState);
        var actionsList = BindFsmState.Actions.ToList();
        var animEventAction = BindFsmState.Actions.FirstOrDefault(action => action is Wait);
        actionsList.Remove(animEventAction);
        actionsList.Insert(0, new Wait()
        {
            finishEvent = FsmEvent.GetFsmEvent("FINISHED"),
            time = 0.5f
        });
        BindFsmState.Actions = actionsList.ToArray();
    }

    public override void SetupPhase1Modifiers()
    {
    }

    public override void SetupPhase2Modifiers()
    {
    }

    public override void SetupPhase3Modifiers()
    {
    }
}