using System.Linq;
using HutongGames.PlayMaker;

namespace KarmelitaPrime;

public class ThrowAnticTransitionerState(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Throw Antic Transitioner";
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
            Actions = [new AnimEndSendRandomEventAction()
            {
                animator = wrapper.animator,
                events = [FsmEvent.GetFsmEvent("FINISHED"), FsmEvent.GetFsmEvent("ATTACK")],
                weights = [.65f, .35f],
                shortenEventTIme = 0.5f
            }],
        };
        fsm.Fsm.States = fsm.Fsm.States.Append(bindState).ToArray();

        BindFsmState.Transitions = [
            new FsmTransition()
            {
                FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                ToState = "Throw Dir",
                ToFsmState = fsm.Fsm.GetState("Throw Dir")
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