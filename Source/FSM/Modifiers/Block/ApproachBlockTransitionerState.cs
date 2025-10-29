using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class ApproachBlockTransitionerState(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Approach Block Transitioner";
    public override void OnCreateModifier()
    {
        var bindState = new FsmState(fsm.Fsm)
        {
            Name = BindState,
            Actions = [
                new WeightedRandomEventAction()
                {
                    events = [FsmEvent.GetFsmEvent("SLASH COMBO"), FsmEvent.GetFsmEvent("CYCLONE SPIN"), FsmEvent.GetFsmEvent("JUMP SPIN")],
                    weights = [.5f, .5f, .0f]
                }
            ],
            Transitions = [
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("EVADE"),
                    ToState = "Evade To Wind Blade",
                    ToFsmState = fsm.Fsm.GetState("Evade To Wind Blade")
                },
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                    ToState = "Generic Teleport Pre",
                    ToFsmState = fsm.Fsm.GetState("Generic Teleport Pre")
                },
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("SLASH COMBO"),
                    ToState = "Slash Antic",
                    ToFsmState = fsm.Fsm.GetState("Slash Antic")
                },
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("CYCLONE SPIN"),
                    ToState = "Cyclone Antic",
                    ToFsmState = fsm.Fsm.GetState("Cyclone Antic")
                },
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("JUMP SPIN"),
                    ToState = "Jump Launch",
                    ToFsmState = fsm.Fsm.GetState("Jump Launch")
                },
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
            events = [FsmEvent.GetFsmEvent("EVADE"), FsmEvent.GetFsmEvent("SLASH COMBO"), FsmEvent.GetFsmEvent("CYCLONE SPIN"), FsmEvent.GetFsmEvent("JUMP SPIN")],
            weights = [.25f, .25f, .25f, .25f]
        };
    }

    public override void SetupPhase3Modifiers()
    {
        BindFsmState.Actions[0] = new WeightedRandomEventAction()
        {
            events = [FsmEvent.GetFsmEvent("FINISHED"), FsmEvent.GetFsmEvent("SLASH COMBO"), FsmEvent.GetFsmEvent("CYCLONE SPIN"), FsmEvent.GetFsmEvent("JUMP SPIN")],
            weights = [.25f, .25f, .25f, .25f]
        };
    }
}