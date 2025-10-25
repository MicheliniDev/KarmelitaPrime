using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine;

namespace KarmelitaPrime;

public class FadeVelocityAction : FsmStateAction
{
    public Rigidbody2D Rb;
    public float Duration;
    
    public override void OnEnter()
    {
        base.OnEnter();
        Fsm.Owner.StartCoroutine(LerpVelocity());
    }

    public override void OnExit()
    {
        base.OnExit();
        Fsm.Owner.StopAllCoroutines();
    }

    private IEnumerator LerpVelocity()
    {
        var rb = Rb;
        float startVelocityX = Rb.linearVelocity.x; 
        float duration = Duration;                 

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            rb.linearVelocityX = Mathf.Lerp(startVelocityX, 0f, t);
            yield return null;
        }
    }
}