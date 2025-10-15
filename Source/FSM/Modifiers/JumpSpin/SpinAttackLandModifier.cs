using System.Linq;
using HutongGames.PlayMaker;

namespace KarmelitaPrime;

public class SpinAttackLandModifier(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Spin Attack Land";
    public override void OnCreateModifier()
    {
    }

    public override void SetupPhase1Modifiers()
    {
        BindFsmState.Transitions = [new FsmTransition()
        {
            FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
            ToState = "Throw Antic",
            ToFsmState = fsm.Fsm.GetState("Throw Antic")
        }];
    }

    public override void SetupPhase2Modifiers()
    {
    }

    public override void SetupPhase3Modifiers()
    {
    }
}