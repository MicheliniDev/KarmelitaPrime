using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

namespace KarmelitaPrime;

public class StateModifierController
{
    private tk2dSpriteAnimator animator;
    private PlayMakerFSM fsm;
    
    private List<StateModifierBase> allModifiers;
    private Dictionary<string, StateModifierBase> stateModifiers;
    
    public static StateModifierController Initialize(tk2dSpriteAnimator animator, PlayMakerFSM fsm)
    {
        StateModifierController instance = new StateModifierController(animator, fsm);
        return instance;
    }
    
    public void ApplyStateModifier(string state, int phaseIndex)
    {
        if (!stateModifiers.ContainsKey(state)) return;
    }

    public float GetSpeedModifier()
    {
        FsmState currentState = fsm.Fsm.GetState(fsm.ActiveStateName);
        if (currentState == null || !stateModifiers.ContainsKey(currentState.Name)) 
            return 1f;
        return stateModifiers[currentState.Name].StateSpeed;
    }
    
    public float GetSpeedModifier(string state)
    {
        if (!stateModifiers.ContainsKey(state)) return 1f;
        return stateModifiers[state].StateSpeed;
    }
    
    private StateModifierController(tk2dSpriteAnimator animator, PlayMakerFSM fsm)
    {
        this.animator = animator;
        this.fsm = fsm;
        allModifiers = new List<StateModifierBase>()
        {
            new Slash1Modifier(),
            new Slash2Modifier(),
            new Slash3Modifier(),
        };
        stateModifiers = new Dictionary<string, StateModifierBase>();
        foreach (var item in allModifiers)
        {
            stateModifiers.Add(item.BindState, item);
        }
    }
}