using HutongGames.PlayMaker;

namespace KarmelitaPrime;

public class AirRethrowModifier(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Air Rethrow";
    public override void OnCreateModifier()
    {
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
            new EnableGameObjectAction()
            {
                GameObject = fsm.Fsm.GetFsmGameObject("Air Slash 2").Value,
                Enable = true,
                ResetOnExit = true,
            },
            new AnimEndSendRandomEventAction()
            {
                animator = wrapper.animator,
                events = [FsmEvent.GetFsmEvent("FINISHED"), FsmEvent.GetFsmEvent("CANCEL")],
                weights = [.5f, .5f],
                shortenEventTIme = 0.73f
            }
        ];
        BindFsmState.Transitions =
        [
            new FsmTransition()
            {
                FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                ToState = "Spin Aim",
                ToFsmState = fsm.Fsm.GetState("Spin Aim")
            },
            new FsmTransition()
            {
                FsmEvent = FsmEvent.GetFsmEvent("CANCEL"),
                ToState = "Generic Teleport",
                ToFsmState = fsm.Fsm.GetState("Generic Teleport")
            }
        ];
    }
}