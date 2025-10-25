using System;
using System.Collections;
using HutongGames.PlayMaker;

namespace KarmelitaPrime;

public class StartCoroutineAction : FsmStateAction
{
    public Func<IEnumerator> Coroutine;

    public override void OnEnter() 
    {
        Fsm.Owner.StartCoroutine(InvokeCoroutine());
    }
    
    private IEnumerator InvokeCoroutine() {
        yield return Coroutine.Invoke();
        Finish();
    }
}