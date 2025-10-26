using System.Linq;
using HutongGames.PlayMaker;

namespace KarmelitaPrime;

public class Slash3TransitionerState(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Slash 3 Transitioner";
    private FsmEvent toNewSlash2State => FsmEvent.GetFsmEvent("SLASH COMBO");
    private FsmEvent toDashGrindEvent => FsmEvent.GetFsmEvent("DASH GRIND");
    private FsmEvent toSlash4State => FsmEvent.GetFsmEvent("ATTACK");
    public override void OnCreateModifier()
    {
        CreateBindState();
    }

    public override void SetupPhase1Modifiers()
    {
        BindFsmState.Transitions =
        [
            new FsmTransition()
            {
                FsmEvent = toSlash4State,
                ToState = "Slash 4",
                ToFsmState = fsm.Fsm.GetState("Slash 4"),
            },
            new FsmTransition()
            {
                FsmEvent = toNewSlash2State,
                ToState = "New Slash 2 State",
                ToFsmState = fsm.Fsm.GetState("New Slash 2 State"),
            }
        ];
    }

    public override void SetupPhase2Modifiers() {}

    public override void SetupPhase3Modifiers()
    {
        BindFsmState.Actions =
        [
            new WeightedRandomEventAction()
            {
                events = [toDashGrindEvent, toNewSlash2State, toSlash4State],
                weights = [.3f, 0.5f, 0.2f],
            }
        ];
        BindFsmState.Transitions = BindFsmState.Transitions.Append
            (
                new FsmTransition()
                {
                    FsmEvent = toDashGrindEvent,
                    ToState = "Set Dash Grind",
                    ToFsmState = fsm.Fsm.GetState("Set Dash Grind"), 
                }
            ).ToArray();
    }

    private void CreateBindState()
    {
        var bindFsmState = new FsmState(fsm.Fsm)
        {
            Name = "Slash 3 Transitioner",
            Actions =
            [
                new WeightedRandomEventAction()
                {
                    events = [toNewSlash2State, toSlash4State],
                    weights = [0.5f, 0.5f],
                }
            ]
        };
        fsm.Fsm.States = fsm.FsmStates.Append(bindFsmState).ToArray();
    }
}