using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine;

namespace KarmelitaPrime;

public class PlayClipDelayedAction : FsmStateAction
{
    public AudioClip Clip;
    public GameObject Source;
    public float Delay;
    public float PitchMin;
    public float PitchMax;
    public override void OnEnter()
    {
        base.OnEnter();
        Fsm.Owner.StartCoroutine(PlayClipDelayedRoutine());
        Finish();
    }

    private IEnumerator PlayClipDelayedRoutine()
    {
        yield return new WaitForSeconds(Delay);
        var source = Source.GetComponent<AudioSource>();
        var audioEvent = new AudioEvent()
        {
            Clip = Clip,
            PitchMin = PitchMin,
            PitchMax = PitchMax,
            Volume = source.volume
        };  
        audioEvent.SpawnAndPlayOneShot(Source.transform.position);
    }
}