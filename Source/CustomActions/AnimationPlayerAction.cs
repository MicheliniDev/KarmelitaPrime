using HutongGames.PlayMaker;
using UnityEngine.InputForUI;

namespace KarmelitaPrime;

public class AnimationPlayerAction : FsmStateAction
{
    public FsmEvent AnimationFinishedEvent;
    public string ClipName;
    public tk2dSpriteAnimator animator;
    
    private tk2dSpriteAnimationClip clip;
    private bool hasSentEvent;
    private float deltaTime;
    
    public override void OnEnter()
    {
        base.OnEnter();
        clip = animator.GetClipByName(ClipName);
        animator.Play(clip, 0f, 0f);
        hasSentEvent = false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        deltaTime += UnityEngine.Time.deltaTime;
        if (deltaTime >= clip.Duration && !hasSentEvent)
        {
            KarmelitaPrimeMain.Instance.Log($"EVENT SENT {AnimationFinishedEvent.Name}");
            Fsm.Event(AnimationFinishedEvent);
            hasSentEvent = true;
        }
    }
}