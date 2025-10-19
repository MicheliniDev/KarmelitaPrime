using HutongGames.PlayMaker;

namespace KarmelitaPrime;

public class AnimationSendEventIfPreviousState : FsmStateAction
{
    public PlayMakerFSM fsm;
    public string StateName;
    public FsmEvent TrueEvent;
    public FsmEvent FalseEvent;
    //ANIMATION
    public string ClipName;
    public tk2dSpriteAnimator Animator;
    public float ShortenEventTIme;
    
    private tk2dSpriteAnimationClip clip;
    private bool hasSentEvent;
    private float deltaTime;
    
    public override void OnEnter()
    {
        base.OnEnter();
        clip = Animator.GetClipByName(ClipName);
        Animator.Play(clip, 0f, 0f);
        hasSentEvent = false;
        deltaTime = 0f;
    }
    
    public override void OnUpdate()
    {
        base.OnUpdate();
        deltaTime += UnityEngine.Time.deltaTime;
        if (deltaTime >= (clip.Duration - ShortenEventTIme) && !hasSentEvent)
        {
            SendEvent();
        }
    }

    private void SendEvent()
    {
        fsm.Fsm.Event(fsm.Fsm.PreviousActiveState.Name == StateName ? TrueEvent : FalseEvent);
        hasSentEvent = true;
    }
}