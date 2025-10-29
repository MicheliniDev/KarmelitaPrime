using HutongGames.PlayMaker;

namespace KarmelitaPrime;

public class ThrowLandModifier(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Throw Land";
    public override void OnCreateModifier()
    {
    }

    public override void SetupPhase1Modifiers()
    {
        BindFsmState.Transitions = [new FsmTransition()
        {
            FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
            ToState = "Cyclone Antic",
            ToFsmState = fsm.Fsm.GetState("Cyclone Antic")
        }];
    }

    public override void SetupPhase2Modifiers()
    {
        BindFsmState.Transitions = [new FsmTransition()
        {
            FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
            ToState = "Evade To Throw",
            ToFsmState = fsm.Fsm.GetState("Evade To Throw")
        }];
    }

    public override void SetupPhase3Modifiers()
    {
    }
}