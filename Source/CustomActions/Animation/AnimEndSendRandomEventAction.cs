using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine;
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

    public bool PlayClipOnEventIndex;
    public int EventIndex;
    public AudioClip EventClip;
    public float ClipDelay;

    private FsmEvent eventToSend;
    public override void OnEnter()
    {
        base.OnEnter();
        hasSentEvent = false;
        elapsedTime = 0f;
        DecideEvent();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        elapsedTime += UnityEngine.Time.deltaTime;
        if (elapsedTime >= (animator.CurrentClip.Duration - shortenEventTIme) && !hasSentEvent)
        {
            Fsm.Event(eventToSend);
            hasSentEvent = true;
        }
    }

    private void DecideEvent()
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
                eventToSend = events[i];
                CheckClip(i);
                break;
            }
        }
    }

    private void CheckClip(int index)
    {
        if (PlayClipOnEventIndex && index == EventIndex)
        {
            var source = Fsm.GetFsmGameObject("Audio Loop Voice").Value.GetComponent<AudioSource>();
            var audioEvent = new AudioEvent()
            {
                Clip = EventClip,
                PitchMin = 1f,
                PitchMax = 1f,
                Volume = source.volume
            };
            Fsm.Owner.StartCoroutine(PlayClipDelay(audioEvent, source));
        }
    }

    private IEnumerator PlayClipDelay(AudioEvent audioEvent, AudioSource source)
    {
        yield return new WaitForSeconds(ClipDelay);
        audioEvent.SpawnAndPlayOneShot(source.transform.position);  
    }
}