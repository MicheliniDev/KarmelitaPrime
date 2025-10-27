using System.Linq;
using HutongGames.PlayMaker;

namespace KarmelitaPrime;

public class Teleport5SickleCheckDirectionState(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Teleport 5 Sickle Check Direction";
    public override void OnCreateModifier()
    {
        FsmState bindState = new FsmState(fsm.Fsm)
        {
            Name = BindState,
            Transitions = [
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("L"),
                    ToState = "Teleport 5 Sickle Throw Prepare Left",
                    ToFsmState = fsm.Fsm.GetState("Teleport 5 Sickle Throw Prepare Left")
                },
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("R"),
                    ToState = "Teleport 5 Sickle Throw Prepare Right",
                    ToFsmState = fsm.Fsm.GetState("Teleport 5 Sickle Throw Prepare Right")
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