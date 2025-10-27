using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace KarmelitaPrime;

public class Teleport5SickleThrowPrepareRightState(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Teleport 5 Sickle Throw Prepare Right";
    public override void OnCreateModifier()
    {
        FsmState bindState = new FsmState(fsm.Fsm)
        {
            Name = BindState,
            Transitions = [
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                    ToState = "Teleport 5 Sickle Throw",
                    ToFsmState = fsm.Fsm.GetState("Teleport 5 Sickle Throw")
                },
            ]
        };
        fsm.Fsm.States = fsm.Fsm.States.Append(bindState).ToArray(); 
        fsmController.CloneActions(fsm.Fsm.GetState("Throw R"), BindFsmState);
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