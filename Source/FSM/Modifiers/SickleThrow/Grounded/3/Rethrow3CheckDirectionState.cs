using System.Linq;
using HutongGames.PlayMaker;

namespace KarmelitaPrime;

public class Rethrow3CheckDirectionState(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Rethrow 3 Check Direction";
    public override void OnCreateModifier()
    {
        FsmState bindState = new FsmState(fsm.Fsm)
        {
            Name = BindState,
            Transitions = [
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("L"),
                    ToState = "Rethrow 3 Prepare Left",
                    ToFsmState = fsm.Fsm.GetState("Rethrow 3 Prepare Left")
                },
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("R"),
                    ToState =  "Rethrow 3 Prepare Right",
                    ToFsmState = fsm.Fsm.GetState("Rethrow 3 Prepare Right")
                }
            ]
        };
        fsm.Fsm.States = fsm.Fsm.States.Append(bindState).ToArray(); 
        fsmController.CloneActions(fsm.Fsm.GetState("Throw Dir"), BindFsmState);
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