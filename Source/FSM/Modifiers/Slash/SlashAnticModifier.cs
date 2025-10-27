using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace KarmelitaPrime;

public class SlashAnticModifier(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Slash Antic";
    public override void OnCreateModifier()
    {
        BindFsmState.Actions = BindFsmState.Actions.Append(new FadeVelocityAction()
        {
            Rb = wrapper.rb,
            Duration = 0.01f,
        }).ToArray();
    }

    public override void SetupPhase1Modifiers()
    {
    }

    public override void SetupPhase2Modifiers()
    {
        BindFsmState.Actions = BindFsmState.Actions.Append(new EnemyHitAction()
        {
            isInvincibleOnEnter = true,
            Owner = wrapper.health,
            OnHitEvent = FsmEvent.GetFsmEvent("BLOCKED HIT"),
            IgnoreHitStartDuration = 0f
        }).ToArray();
        BindFsmState.Transitions = BindFsmState.Transitions.Append(new FsmTransition()
        {
            FsmEvent = FsmEvent.GetFsmEvent("BLOCKED HIT"),
            ToState = "Counter Attack",
            ToFsmState = fsm.Fsm.GetState("Counter Attack Pre")
        }).ToArray();
    }

    public override void SetupPhase3Modifiers()
    {
    }
}