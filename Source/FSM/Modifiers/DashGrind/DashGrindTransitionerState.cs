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
    }
    
    private void CreateBindState()
    {
        FsmState bindState = new FsmState(fsm.Fsm)
        {
            Name = BindState,
            Actions = [new WeightedRandomEventAction()
            {
                events = [FsmEvent.GetFsmEvent("FINISHED"), FsmEvent.GetFsmEvent("THROW")],
                weights = [.5f, .5f],
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
                FsmEvent = FsmEvent.GetFsmEvent("THROW"),
                ToState = "Throw Antic",
                ToFsmState = fsm.Fsm.GetState("Throw Antic")
            }
        ];
    }
}