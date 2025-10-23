using HutongGames.PlayMaker;
using UnityEngine;

namespace KarmelitaPrime;

public class PlayClipAction : FsmStateAction
{
    public AudioClip Clip;
    public GameObject Source;

    public override void OnEnter()
    {
        base.OnEnter();
        var source = Source.GetComponent<AudioSource>();
        var audioEvent = new AudioEvent()
        {
            Clip = Clip,
            PitchMin = 1f,
            PitchMax = 1f,
            Volume = source.volume
        };  
        audioEvent.SpawnAndPlayOneShot(Source.transform.position);
    }
}