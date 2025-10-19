using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class NewSlash2State(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "New Slash 2 State";
    public override float AnimationStartTime => 0f;
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
                ToState = "Jump Antic",
                ToFsmState = fsm.Fsm.GetState("Jump Antic")
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
            Name = "New Slash 2 State",
            Actions = [new AnimationPlayerAction()
            {
                ClipName = "Slash 2",
                animator = wrapper.animator,
                AnimationFinishedEvent = finishedEvent,
            }],
        };
        fsm.Fsm.States = fsm.Fsm.States.Append(bindState).ToArray();
    }
}