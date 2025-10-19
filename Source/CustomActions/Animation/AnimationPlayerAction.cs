using HutongGames.PlayMaker;
using UnityEngine.InputForUI;

namespace KarmelitaPrime;

public class AnimationPlayerAction : FsmStateAction
{
    public FsmEvent AnimationFinishedEvent;
    public string ClipName;
    public tk2dSpriteAnimator animator;
    public float shortenEventTIme;
    
    private tk2dSpriteAnimationClip clip;
    private bool hasSentEvent;
    private float deltaTime;
    
    public override void OnEnter()
    {
        base.OnEnter();
        clip = animator.GetClipByName(ClipName);
        animator.Play(clip, 0f, 0f);
        hasSentEvent = false;
        deltaTime = 0f;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        deltaTime += UnityEngine.Time.deltaTime;
        if (deltaTime >= (clip.Duration - shortenEventTIme) && !hasSentEvent)
        {
            Fsm.Event(AnimationFinishedEvent);
            hasSentEvent = true;
        }
    }
}