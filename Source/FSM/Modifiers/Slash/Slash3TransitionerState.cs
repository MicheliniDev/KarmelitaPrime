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
    private FsmEvent toSlash4State => FsmEvent.GetFsmEvent("ATTACK");
    public override void OnCreateModifier()
    {
        CreateBindState();
    }

    public override void SetupPhase1Modifiers()
    {
    }

    public override void SetupPhase2Modifiers() {}

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
                    events = [toSlash4State], //toNewSlash2State, toSlash4State
                    weights = [1f], //0.5f, 0.5f
                }
            ],
            Transitions =
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
            ]
        };
        fsm.Fsm.States = fsm.FsmStates.Append(bindFsmState).ToArray();
    }
}