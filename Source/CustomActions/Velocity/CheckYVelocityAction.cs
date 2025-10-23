using HutongGames.PlayMaker;
using UnityEngine;

namespace KarmelitaPrime;

public class CheckYVelocityAction : FsmStateAction
{
    public Rigidbody2D Rb;
    public float Velocity;
    public FsmEvent OnVelocityMatch;
    private bool hasSentEvent;
    public override void OnEnter()
    {
        base.OnEnter();
        hasSentEvent = false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Mathf.Abs(Rb.linearVelocityY) <= Velocity && !hasSentEvent)
        {
            Fsm.Event(OnVelocityMatch);
            hasSentEvent = true;
        }
    }
}