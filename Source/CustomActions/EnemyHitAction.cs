using HutongGames.PlayMaker;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace KarmelitaPrime;

public class EnemyHitAction : FsmStateAction
{
    public HealthManager Owner;
    public FsmEvent OnHitEvent;
    public bool isInvincibleOnEnter;
    public float IgnoreHitStartDuration;
    private float elapsedTime;
    private float hpOnEnter;
    private bool hasSentEvent;
    public override void OnEnter()
    {
        base.OnEnter();
        elapsedTime = 0f;
        hasSentEvent = false;
        hpOnEnter = Owner.hp;
        Owner.IsInvincible = isInvincibleOnEnter;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        elapsedTime += Time.deltaTime;
        if (Owner.hp < hpOnEnter && !hasSentEvent && OnHitEvent != null && elapsedTime >= IgnoreHitStartDuration)
        {
            Fsm.Event(OnHitEvent);
            hasSentEvent = true;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        Owner.IsInvincible = false;
    }
}