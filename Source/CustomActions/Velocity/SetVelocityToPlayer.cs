using HutongGames.PlayMaker;
using UnityEngine;

namespace KarmelitaPrime;

public class SetVelocityToPlayer : FsmStateAction
{
    public Rigidbody2D Rb;
    public float velocity;
    public float velocityY;

    public override void OnEnter()
    {
        base.OnEnter();
        var scale = Rb.gameObject.transform.localScale;
        var xScale = scale.x;

        var xDistance = HeroController.instance.transform.position.x - Rb.gameObject.transform.position.x;

        if ((xDistance > 0f && xScale > 0f) || (xDistance < 0f && xScale < 0f))
        {
            scale.x *= -1f;
        }

        Rb.gameObject.transform.localScale = scale;
        Rb.linearVelocityX = velocity * (Rb.gameObject.transform.localScale.x < 0 ? 1 : -1);
        Rb.linearVelocityY = velocityY;
    }
}