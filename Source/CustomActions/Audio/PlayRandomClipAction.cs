using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace KarmelitaPrime;

public class PlayRandomClipAction : FsmStateAction
{
    public RandomAudioClipTable Table;
    public GameObject Source;
    public override void OnEnter()
    {
        base.OnEnter();
        if (!Table || !Source) return;

        var source = Source.GetComponent<AudioSource>();
        Table.PlayOneShot(source);
    }
}