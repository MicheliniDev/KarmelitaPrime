using HutongGames.PlayMaker;

namespace KarmelitaPrime;

public class ApproachBlockModifier(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Approach Block";
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
        BindFsmState.Transitions = [new FsmTransition()
        {
            FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
            ToState = "Generic Teleport Pre",
            ToFsmState = fsm.Fsm.GetState("Generic Teleport Pre")
        }];
    }
}