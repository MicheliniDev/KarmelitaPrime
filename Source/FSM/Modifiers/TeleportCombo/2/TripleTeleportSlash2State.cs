using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime.TripleTeleportSlash;

public class TripleTeleportSlash2State(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Triple Teleport Slash 2";
    public override void OnCreateModifier()
    {
        FsmState bindState = new FsmState(fsm.Fsm)
        {
            Name = BindState,
            Transitions = [
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                    ToState = "Teleport 3 Pre",
                    ToFsmState = fsm.Fsm.GetState("Teleport 3 Pre")
                }
            ]
        };
        fsm.Fsm.States = fsm.Fsm.States.Append(bindState).ToArray();
        fsmController.CloneActions(fsm.Fsm.GetState("Slash 4"), BindFsmState);
        var actionsList = BindFsmState.Actions.ToList();
        var animEventAction = BindFsmState.Actions.FirstOrDefault(action => action is Tk2dPlayAnimationWithEvents);
        actionsList.Remove(animEventAction);
        actionsList.InsertRange(0, 
        [
            new AnimationPlayerAction()
            {
                animator = wrapper.animator,
                ClipName = "Slash 2",
                AnimationFinishedEvent = FsmEvent.GetFsmEvent("FINISHED"),
                shortenEventTIme = 0.15f
            }, 
            new EnableGameObjectAction()
            {
                GameObject = fsm.Fsm.GetFsmGameObject("Slash 2").Value,
                Enable = true,
                ResetOnExit = true
            }
        ]);
        BindFsmState.Actions = actionsList.ToArray();
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