using System;
using HutongGames.PlayMaker;
using UnityEngine;

namespace KarmelitaPrime;

public class SetVelocityByAngleAction : FsmStateAction
{
    public GameObject Rb;
    public float Speed;
    public Transform OwnerTransform;
    public float AngleLeft;
    public float AngleRight;
    public override void OnEnter()
    {
        base.OnEnter();

        if (Rb == null) return;
        var rb = Rb.GetComponent<Rigidbody2D>();
        
        bool isFacingRight = OwnerTransform.localScale.x < 0f;
        float angleToUse = isFacingRight ? AngleRight : AngleLeft;
        
        rb.linearVelocity = new Vector2(
            Speed * Mathf.Cos(angleToUse * ((float)Math.PI / 180f)),
            Speed * Mathf.Sin(angleToUse * ((float)Math.PI / 180f)));
        Finish();
    }
}