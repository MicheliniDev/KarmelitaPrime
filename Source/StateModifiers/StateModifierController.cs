using System.Collections.Generic;
using UnityEngine;

namespace KarmelitaPrime;

public class StateModifierController
{
    private tk2dSpriteAnimator animator;
    private PlayMakerFSM fsm;
    private List<StateModifier> allModifiers;
    private Dictionary<string, StateModifier> stateModifiers;
    private int originalFps = 12;
    public void ApplyStateModifier(string state, int phaseIndex)
    {
        if (!stateModifiers.ContainsKey(state)) return;
        TryDoSpeedModifier(state);
    }

    private void TryDoSpeedModifier(string state)
    {
        ApplyAnimatorSpeed(stateModifiers[state].StateSpeed);
    }

    private StateModifierController(tk2dSpriteAnimator animator, PlayMakerFSM fsm)
    {
        this.animator = animator;
        this.fsm = fsm;
        allModifiers = new List<StateModifier>()
        {
            new Slash1Modifier(),
        };
        stateModifiers = new Dictionary<string, StateModifier>();
        foreach (var item in allModifiers)
        {
            stateModifiers.Add(item.BindState, item);
        }
    }

    public static StateModifierController Initialize(tk2dSpriteAnimator animator, PlayMakerFSM fsm)
    {
        StateModifierController instance = new StateModifierController(animator, fsm);
        return instance;
    }

    private void ApplyAnimatorSpeed(float speed)
    {
        animator.ClipFps = originalFps;
        animator.ClipFps *= speed;
    }
}