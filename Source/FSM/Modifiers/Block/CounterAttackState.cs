using System.Collections;
using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace KarmelitaPrime;

public class CounterAttackState(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Counter Attack";
    public override void OnCreateModifier()
    {
        var bindState = new FsmState(fsm.Fsm)
        {
            Name = "Counter Attack",
            Actions = [
                new StartCoroutineAction()
                {
                    Coroutine = LerpVelocity
                },
                new AnimationPlayerAction()
                {
                    animator = wrapper.animator,
                    ClipName = "Dash Grind Spin"
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
                    Clip = wrapper.CycloneClip,
                    Source = fsm.Fsm.GetFsmGameObject("Audio Loop Voice").Value
                },
                new EnableGameObjectAction()
                {
                    GameObject = fsm.Fsm.GetFsmGameObject("SpinSlash 1").Value,
                    ResetOnExit = true
                }
            ],
            Transitions = [
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                    ToState = "New Slash 2 State",
                    ToFsmState = fsm.Fsm.GetState("New Slash 2 State")
                }
            ]
        };
        fsm.Fsm.States = fsm.Fsm.States.Append(bindState).ToArray();
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
    
    private IEnumerator LerpVelocity()
    {
        var rb = wrapper.rb;
        Vector2 direction = Vector2.right * -wrapper.transform.localScale.x;
        
        float maxSpeed = 80f;                
        float duration = 0.4f;                 
    
        float halfDuration = duration / 2f;
        float accelerateDuration = halfDuration + (duration * 0.10f);
        float decelerateDuration = halfDuration + (duration * 0.90f);
        float elapsed = 0f;

        while (elapsed < accelerateDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / accelerateDuration;
            rb.linearVelocity = direction.normalized * Mathf.Lerp(0f, maxSpeed, t);
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < decelerateDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / decelerateDuration;
            rb.linearVelocity = direction.normalized * Mathf.Lerp(maxSpeed, 0f, t);
            yield return null;
        }

        rb.linearVelocity = Vector3.zero;
    }
}