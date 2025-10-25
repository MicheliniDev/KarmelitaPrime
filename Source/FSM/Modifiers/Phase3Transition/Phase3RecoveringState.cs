using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace KarmelitaPrime;

public class Phase3RecoveringState(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Phase 3 Recovering State";
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
        var bindFsmState = new FsmState(fsm.Fsm)
        {
            Name = "Phase 3 Recovering State",
            Actions = [
                new FadeVelocityAction()
                {
                    Rb = wrapper.rb,
                    Duration = 0.2f
                },
                new EnemyHitAction()
                {
                    Owner = wrapper.health,
                    isInvincibleOnEnter = true,
                },
                new PlayClipAction()
                {
                    Clip = wrapper.LandClip,
                    Source = fsm.Fsm.GetFsmGameObject("Audio Loop Voice").Value
                },
                new AnimationPlayerAction()
                {
                    animator = wrapper.animator,
                    ClipName = "Stun Land"
                },
                new SetVelocity()
                {
                    vector = Vector3.zero,
                    x = 0f,
                    y = 0f
                },
                new Wait()
                {
                    time = 2f,
                    finishEvent = FsmEvent.GetFsmEvent("FINISHED")
                },
                new SetAllMusicSources()
                {
                    Active = true,
                    IsOnExit = true
                }
            ],
            Transitions = [new FsmTransition()
            {
                FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                ToState = "P3 Roar Antic",
                ToFsmState = fsm.Fsm.GetState("P3 Roar Antic")
            }]
        };
        fsm.Fsm.States = fsm.FsmStates.Append(bindFsmState).ToArray();
    }
}