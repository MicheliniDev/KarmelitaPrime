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
    private FsmEvent finishedEvent => FsmEvent.GetFsmEvent("FINISHED");
    private FsmEvent throwEvent => FsmEvent.GetFsmEvent("THROW");
    
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
            },
            new FsmTransition()
            {
                FsmEvent = throwEvent,
                ToState = "Throw Antic",
                ToFsmState = fsm.Fsm.GetState("Throw Antic")
            }
        ];
    }

    public override void SetupPhase2Modifiers()
    {
        for (int i = 0; i < BindFsmState.Actions.Length; i++)
        {
            if (BindFsmState.Actions[i] is AnimEndSendRandomEventAction animEnd)
            {
                animEnd.events = [finishedEvent, throwEvent];
                animEnd.weights = [0.5f, 0.5f];
                BindFsmState.Actions[i] = animEnd;
            }
        }
    }

    public override void SetupPhase3Modifiers()
    {
    }
    
    private void CreateBindState()
    {
        FsmState bindState = new FsmState(fsm.Fsm)
        {
            Name = "New Slash 2 State",
            Actions = [
                new AnimationPlayerAction()
                {
                    ClipName = "Slash 2",
                    animator = wrapper.animator,
                    AnimationFinishedEvent = finishedEvent,
                }, 
                new AnimEndSendRandomEventAction()
                {
                    animator = wrapper.animator,
                    events = [finishedEvent],
                    weights = [1f]
                },
                new SetVelocityToPlayer()
                {
                    Rb = wrapper.rb,    
                    velocity = 50f
                },
                new FadeVelocityAction()
                {
                    Rb = wrapper.rb,
                    Duration = .8f,
                },
                new PlayRandomClipAction()
                {
                    Table = wrapper.AttackQuickTable,
                    Source = fsm.Fsm.GetFsmGameObject("Audio Loop Voice").Value
                },
                new PlayClipAction()
                {
                    Clip = wrapper.SwordClip,
                    Source = fsm.Fsm.GetFsmGameObject("Audio Loop Voice").Value
                },
                new EnableGameObjectAction()
                {
                    GameObject = fsm.Fsm.GetFsmGameObject("Slash 2").Value,
                    Enable = true,
                    ResetOnExit = true
                }
            ],
        };
        fsm.Fsm.States = fsm.Fsm.States.Append(bindState).ToArray();
    }
}