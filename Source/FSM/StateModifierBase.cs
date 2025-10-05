using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace KarmelitaPrime;

public abstract class StateModifierBase
{
    public abstract string BindState { get; }
    public virtual float AnimationStartTime => 0f;
    
    protected KarmelitaPrimeMain Main => KarmelitaPrimeMain.Instance;
    
    protected FsmState BindFsmState => fsm.Fsm.GetState(BindState);
    
    protected FsmEventTarget karmelitaEventTarget;
    
    protected PlayMakerFSM fsm;
    protected PlayMakerFSM stunFsm;
    protected KarmelitaWrapper wrapper;
    protected KarmelitaFsmController fsmController;

    protected StateModifierBase(PlayMakerFSM fsm, PlayMakerFSM stunFsm, KarmelitaWrapper wrapper,
        KarmelitaFsmController fsmController)
    {
        this.fsm = fsm;
        this.stunFsm = stunFsm;
        this.wrapper = wrapper;
        this.fsmController = fsmController;
        MakeEventTarget();
    }

    public abstract void OnCreateModifier();
    public abstract void SetupPhase1Modifiers();
    public abstract void SetupPhase2Modifiers();
    public abstract void SetupPhase3Modifiers();

    private void MakeEventTarget()
    {
        karmelitaEventTarget = new FsmEventTarget
        {
            target = FsmEventTarget.EventTarget.GameObject,
            excludeSelf = false,
            gameObject = null,
            fsmName = fsm.FsmName,
            sendToChildren = false,
            fsmComponent = fsm,
        };
    }
}