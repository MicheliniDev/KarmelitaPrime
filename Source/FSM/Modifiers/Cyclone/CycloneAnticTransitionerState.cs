using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class CycloneAnticTransitionerState(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Cyclone Antic Transitioner State";
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
        var bindState = new FsmState(fsm.Fsm)
        {
            Name = BindState,
            Actions = [new WeightedRandomEventAction()
            {
                events = [FsmEvent.GetFsmEvent("FINISHED"), FsmEvent.GetFsmEvent("ATTACK")],
                weights = [.65f, .35f],
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
                FsmEvent = FsmEvent.GetFsmEvent("ATTACK"),
                ToState = "Cyclone 1",
                ToFsmState = fsm.Fsm.GetState("Cyclone 1")
            }
        ];
    }
}