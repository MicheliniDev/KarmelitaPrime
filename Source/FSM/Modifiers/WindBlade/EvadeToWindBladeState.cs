using System.Collections;
using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace KarmelitaPrime;

public class EvadeToWindBladeState(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Evade To Wind Blade";
    public override void OnCreateModifier()
    {
        FsmState bindState = new FsmState(fsm.Fsm)
        {
            Name = BindState,
            Actions = 
            [
                new StartCoroutineAction()
                {
                    Coroutine = DoDashVelocity
                },
                new AnimationPlayerAction()
                {
                    animator = wrapper.animator,
                    ClipName = "Dash",
                    AnimationFinishedEvent = FsmEvent.GetFsmEvent("FINISHED")
                },
            ],
            Transitions = [
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                    ToState = "Wind Blade",
                    ToFsmState = fsm.Fsm.GetState("Wind Blade")
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

    private IEnumerator DoDashVelocity()
    {
        var rb = wrapper.rb;
        var transform = rb.transform;

        float dashSpeed = 60f;
        float dashDuration = .5f;
        
        Vector2 playerPos = HeroController.instance.transform.position;
        Vector2 enemyPos = transform.position;

        bool playerRight = playerPos.x > enemyPos.x;

        bool facingRight = transform.localScale.x < 0;

        bool shouldFaceRight = !playerRight;
        if (facingRight != shouldFaceRight)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }

        rb.linearVelocityX = dashSpeed * -transform.localScale.normalized.x;
        yield return new WaitForSeconds(dashDuration);
        rb.linearVelocityX = 0f;
    }
}