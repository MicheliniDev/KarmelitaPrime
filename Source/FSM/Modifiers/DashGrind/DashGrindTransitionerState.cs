using System.Linq;
using HutongGames.PlayMaker;

namespace KarmelitaPrime;

public class DashGrindTransitionerState(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Dash Grind Transitioner";
    public override void OnCreateModifier()
    {
        CreateBindState();
    }

    public override void SetupPhase1Modifiers()
    {
    }

    public override void SetupPhase2Modifiers()
    {
    }

    public override void SetupPhase3Modifiers()
    {
        BindFsmState.Actions =
        [
            new WeightedRandomEventAction()
            {
                events = [FsmEvent.GetFsmEvent("FINISHED"), FsmEvent.GetFsmEvent("CANCEL")],
                weights = [.5f, .5f],
            }
        ];
    }
    
    private void CreateBindState()
    {
        FsmState bindState = new FsmState(fsm.Fsm)
        {
            Name = BindState,
            Actions = [new WeightedRandomEventAction()
            {
                events = [FsmEvent.GetFsmEvent("FINISHED"), FsmEvent.GetFsmEvent("THROW")],
                weights = [1f, 0f],
            }],
        };
        fsm.Fsm.States = fsm.Fsm.States.Append(bindState).ToArray();

        BindFsmState.Transitions = [
            new FsmTransition()
            {
                FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                ToState = "Jump Launch",
                ToFsmState = fsm.Fsm.GetState("Jump Launch")
            },
            new FsmTransition()
            {
                FsmEvent = FsmEvent.GetFsmEvent("CANCEL"),
                ToState = "Generic Teleport Pre",
                ToFsmState = fsm.Fsm.GetState("Generic Teleport Pre")
            },
        ];
    }
}