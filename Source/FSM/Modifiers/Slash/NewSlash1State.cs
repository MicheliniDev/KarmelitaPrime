using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class NewSlash1State(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "New Slash 1 State";
    public override float AnimationStartTime => 0f;
    FsmEvent finishedEvent => FsmEvent.GetFsmEvent("FINISHED");
    FsmEvent longEvent => FsmEvent.GetFsmEvent("LONG");
    FsmEvent regularEvent => FsmEvent.GetFsmEvent("ATTACK");
    
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
                ToState = "Set Air Throw",
                ToFsmState = fsm.Fsm.GetState("Set Air Throw")
            },
            new FsmTransition()
            {
                FsmEvent = longEvent,
                ToState = "Cyclone Antic",
                ToFsmState = fsm.Fsm.GetState("Cyclone Antic")
            },
            new FsmTransition()
            {
                FsmEvent = regularEvent,
                ToState = "New Slash 2 State",
                ToFsmState = fsm.Fsm.GetState("New Slash 2 State")
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
            Name = "New Slash 1 State",
            Actions = [
                new AnimationPlayerAction()
                {
                    ClipName = "Slash 1",
                    animator = wrapper.animator,
                },
                new AnimEndSendRandomEventAction()
                {
                    animator = wrapper.animator,
                    events = [longEvent, finishedEvent, regularEvent],
                    weights = [.35f, .35f, .3f]
                },
                new SetVelocityToPlayer()
                {
                    Rb = wrapper.rb,
                    velocity = 60f
                },
                new DecelerateXY()
                {
                    gameObject = new FsmOwnerDefault() {OwnerOption = OwnerDefaultOption.UseOwner},
                    decelerationX = 0.9f,
                    decelerationY = 0.9f
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
                    GameObject = fsm.Fsm.GetFsmGameObject("Slash 1").Value,
                    Enable = true,
                    ResetOnExit = true
                }
            ],
        };
        fsm.Fsm.States = fsm.Fsm.States.Append(bindState).ToArray();
    }
}