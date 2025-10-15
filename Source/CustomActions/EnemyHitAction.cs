using HutongGames.PlayMaker;
using UnityEngine.PlayerLoop;

namespace KarmelitaPrime;

public class EnemyHitAction : FsmStateAction
{
    public HealthManager Owner;
    public FsmEvent OnHitEvent;
    public bool isInvincibleOnEnter;
    private float hpOnEnter;
    public override void OnEnter()
    {
        Owner.IsInvincible = false;
        base.OnEnter();
        hpOnEnter = Owner.hp;
        Owner.IsInvincible = isInvincibleOnEnter;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Owner.hp < hpOnEnter)
        {
            Fsm.Event(OnHitEvent);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        Owner.IsInvincible = false;
    }
}