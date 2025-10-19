using HutongGames.PlayMaker;
using UnityEngine.InputForUI;

namespace KarmelitaPrime;

public class AnimEndSendRandomEventAction : FsmStateAction
{
    public tk2dSpriteAnimator animator;
    public FsmEvent[] events;
    public float[] weights;
    public float shortenEventTIme;
    
    private bool hasSentEvent;
    private float elapsedTime;
    public override void OnEnter()
    {
        base.OnEnter();
        hasSentEvent = false;
        elapsedTime = 0f;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        elapsedTime += UnityEngine.Time.deltaTime;
        if (elapsedTime >= (animator.CurrentClip.Duration - shortenEventTIme) && !hasSentEvent)
        {
            SendEvent();
        }
    }

    private void SendEvent()
    {
        float total = 0;
        
        for (int i = 0; i < weights.Length; i++) 
            total += weights[i];
        
        float roll = UnityEngine.Random.value * total;
        float cumulative = 0f;
        for (int i = 0; i < events.Length; i++)
        {
            cumulative += weights[i];
            if (roll <= cumulative)
            {
                Fsm.Event(events[i]);
                hasSentEvent = true;
                break;
            }
        }
    }
}