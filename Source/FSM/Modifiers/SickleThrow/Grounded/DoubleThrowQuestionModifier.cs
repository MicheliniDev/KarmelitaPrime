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
        BindFsmState.Transitions = [
            new FsmTransition()
            {
                FsmEvent = FsmEvent.GetFsmEvent("CANCEL"),
                ToState = "Evade",
                ToFsmState = fsm.Fsm.GetState("Evade"),
            },
            new FsmTransition()
            {
                FsmEvent = FsmEvent.GetFsmEvent("THROW"),
                ToState = "Rethrow Antic 1",
                ToFsmState = fsm.Fsm.GetState("Rethrow Antic 1"),
            },
            new FsmTransition()
            {
                FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                ToState = "Cyclone 1",
                ToFsmState = fsm.Fsm.GetState("Cyclone 1"),
            }
        ];
        
        BindFsmState.Actions = BindFsmState.Actions.Prepend(
            new CheckHeroTooCloseAction()
            {
                Owner = wrapper.gameObject,
                Threshold = 5f,
                TrueEvent = FsmEvent.GetFsmEvent("CANCEL") 
            }).ToArray();
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