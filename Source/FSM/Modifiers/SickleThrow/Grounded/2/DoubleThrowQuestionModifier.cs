using System.Linq;
using HutongGames.PlayMaker;

namespace KarmelitaPrime;

public class DoubleThrowQuestionModifier(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Double Throw?";
    public override void OnCreateModifier()
    {
        /*BindFsmState.Transitions = BindFsmState.Transitions.Prepend(new FsmTransition()
        {
            FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
            ToState = "Evade",
            ToFsmState = fsm.Fsm.GetState("Evade"),
        }).ToArray();
        
        BindFsmState.Actions = BindFsmState.Actions.Prepend(
            new CheckHeroTooCloseAction()
            {
                Owner = wrapper.gameObject,
                Threshold = 3f,
                TrueEvent = FsmEvent.GetFsmEvent("FINISHED") 
            }).ToArray();*/
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