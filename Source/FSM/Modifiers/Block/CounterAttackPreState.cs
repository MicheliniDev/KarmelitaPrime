using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class CounterAttackPreState(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Counter Attack Pre";
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
            Name = "Counter Attack Pre",
            Actions = [
                new AnimationPlayerAction()
                {
                    animator = wrapper.animator,
                    ClipName = "Block 1",
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
                    ToState = "Counter Attack",
                    ToFsmState = fsm.Fsm.GetState("Counter Attack")
                }
            ]
        };
        fsm.Fsm.States = fsm.Fsm.States.Append(bindState).ToArray();
    }
}