using HutongGames.PlayMaker;

namespace KarmelitaPrime;

public class ThrowAnticModifier(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Throw Antic";
    public override void OnCreateModifier()
    {
        BindFsmState.Transitions = [
            new FsmTransition()
            {
                FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                ToState = "Throw Antic Transitioner",
                ToFsmState = fsm.Fsm.GetState("Throw Antic Transitioner"),
            }
        ];
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