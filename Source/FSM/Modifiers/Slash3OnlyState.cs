using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class Slash3OnlyState(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Slash3OnlyState";
    public override float AnimationStartTime => 0.3f;
    FsmEvent finishedEvent => FsmEvent.GetFsmEvent("FINISHED");
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
                FsmEvent = finishedEvent,
                ToState = "Set Dash Grind",
                ToFsmState = fsm.Fsm.GetState("Set Dash Grind")
            }
        ];
    }

    public override void SetupPhase2Modifiers()
    {
    }

    public override void SetupPhase3Modifiers()
    {
    }
    
    private void CreateBindState()
    {
        FsmState bindState = new FsmState(fsm.Fsm)
        {
            Name = "Slash3OnlyState",
            Actions = [new AnimationPlayerAction()
            {
                ClipName = "Slash 1",
                animator = wrapper.animator,
                AnimationFinishedEvent = finishedEvent
            }],
        };
        fsm.Fsm.States = fsm.Fsm.States.Append(bindState).ToArray();
    }
}