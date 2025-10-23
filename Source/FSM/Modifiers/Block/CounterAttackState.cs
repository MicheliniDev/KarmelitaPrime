using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class CounterAttackState(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Counter Attack";
    public override void OnCreateModifier()
    {
        CreateBindState();
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

    private void CreateBindState()
    {
        var bindState = new FsmState(fsm.Fsm)
        {
            Name = "Counter Attack",
            Actions = [
                new SetVelocityToPlayer()
                {
                    Rb = wrapper.rb,
                    velocity = 80f,
                },
                new DecelerateXY()
                {
                    decelerationX = 0.6f,
                    decelerationY = 0.9f
                },
                new AnimationPlayerAction()
                {
                    animator = wrapper.animator,
                    ClipName = "Dash Grind Spin"
                },
                new Wait()
                {
                    time = 0.4f,
                    finishEvent = FsmEvent.GetFsmEvent("FINISHED")
                },
                new PlayRandomClipAction()
                {
                    Table = wrapper.AttackLongTable,
                    Source = fsm.Fsm.GetFsmGameObject("Audio Loop Voice").Value
                },
                new PlayClipAction()
                {
                    Clip = wrapper.SwordClip,
                    Source = fsm.Fsm.GetFsmGameObject("Audio Loop Voice").Value
                }
            ],
            Transitions = [
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                    ToState = "Throw Antic",
                    ToFsmState = fsm.Fsm.GetState("Throw Antic")
                }
            ]
        };
        fsm.Fsm.States = fsm.Fsm.States.Append(bindState).ToArray();
    }
}