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
                FsmEvent = toDashGrindEvent,
                ToState = "Set Dash Grind",
                ToFsmState = fsm.Fsm.GetState("Set Dash Grind"),
            },
            new FsmTransition()
            {
                FsmEvent = toSlash4State,
                ToState = "Slash 4",
                ToFsmState = fsm.Fsm.GetState("Slash 4"),
            },
        ];
    }

    public override void SetupPhase2Modifiers()
    {
        BindFsmState.Transitions = BindFsmState.Transitions.Append(
            new FsmTransition()
            {
                FsmEvent = toNewSlash2State,
                ToState = "New Slash 2 State",
                ToFsmState = fsm.Fsm.GetState("New Slash 2 State"),
            }).ToArray();
    }

    public override void SetupPhase3Modifiers()
    {
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
                    weights = [0f, 1f],
                }
            ]
        };
        fsm.Fsm.States = fsm.FsmStates.Append(bindFsmState).ToArray();
    }
}