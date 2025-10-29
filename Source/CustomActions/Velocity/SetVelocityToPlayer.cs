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
        //SAME DIRECTION
        if (HeroController.instance.transform.position.x > Rb.transform.position.x &&
            Rb.transform.localScale.x > 0 ||
            HeroController.instance.transform.position.x < Rb.transform.position.x &&
            Rb.transform.localScale.x < 0)
        {
            Vector3 scale = Rb.transform.localScale;
            scale.x *= -1f;
            Rb.transform.localScale = scale;
        }
        Rb.linearVelocityX = velocity * -Rb.gameObject.transform.localScale.normalized.x;  
        Rb.linearVelocityY = velocityY;
        Finish();
    }
}