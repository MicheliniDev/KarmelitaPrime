using HutongGames.PlayMaker;

namespace KarmelitaPrime;

public class DashGrindModifier(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Dash Grind";
    public override void OnCreateModifier() 
    {
        BindFsmState.Transitions = [new FsmTransition()
        {
            FsmEvent = FsmEvent.GetFsmEvent("WALL"),
            ToState = "Dash Grind Transitioner",
            ToFsmState = fsm.Fsm.GetState("Dash Grind Transitioner")
        }];
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