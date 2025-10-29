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
        var bindState = new FsmState(fsm.Fsm)
        {
            Name = BindState,
            Actions = 
            [
                new CheckHeroYAction()
                {
                    Target = wrapper.transform,
                    Threshold = 4f,
                    AboveEvent = FsmEvent.GetFsmEvent("FINISHED"),
                    BelowEvent = FsmEvent.GetFsmEvent("ATTACK"),
                }
            ],
            Transitions = [
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
                },
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("EVADE"),
                    ToState = "Evade To Wind Blade",
                    ToFsmState = fsm.Fsm.GetState("Evade To Wind Blade")
                }
            ]
        };
        fsm.Fsm.States = fsm.Fsm.States.Append(bindState).ToArray();
    }

    public override void SetupPhase1Modifiers()
    {
    }

    public override void SetupPhase2Modifiers()
    {
        BindFsmState.Actions[0] = new WeightedRandomEventAction()
        {
            events = [FsmEvent.GetFsmEvent("ATTACK"), FsmEvent.GetFsmEvent("EVADE")],
            weights = [6f, .4f],
        };
    }

    public override void SetupPhase3Modifiers()
    {
        BindFsmState.Actions[0] = new WeightedRandomEventAction()
        {
            events = [FsmEvent.GetFsmEvent("ATTACK")],
            weights = [1f],
        };
    }
}