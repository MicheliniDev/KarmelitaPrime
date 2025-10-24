using System.Collections.Generic;
using System.Linq;
using GenericVariableExtension;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using KarmelitaPrime.TripleTeleportSlash;
using Mono.Cecil;
using UnityEngine;

namespace KarmelitaPrime;

public class KarmelitaFsmController(PlayMakerFSM fsm, PlayMakerFSM stunFsm, KarmelitaWrapper wrapper)
{
    private KarmelitaPrimeMain main => KarmelitaPrimeMain.Instance;
    
    private readonly PlayMakerFSM fsm = fsm;
    private readonly PlayMakerFSM stunFsm = stunFsm;
    private readonly KarmelitaWrapper wrapper = wrapper;
    
    private List<StateModifierBase> stateModifiers;
    private Dictionary<string, StateModifierBase> stateModifierCollection = new();
    
    public string NextMove
    {
        get => fsm.FsmVariables.FindFsmString("Next Move").Value;
        set => fsm.FsmVariables.FindFsmString("Next Move").Value = value;
    }

    public void Initialize()
    {
        RerouteFirstRoarState();
        SubscribeStateChangedEvent();
        stateModifiers = [
            new CounterAttackState(fsm, stunFsm, wrapper, this),
            new ApproachBlockModifier(fsm, stunFsm, wrapper, this),
            new SlashAnticModifier(fsm, stunFsm, wrapper, this),
            new NewSlash1State(fsm, stunFsm, wrapper, this),
            new NewSlash2State(fsm, stunFsm, wrapper, this),
            new Slash1Modifier(fsm, stunFsm, wrapper, this),
            new Slash3TransitionerState(fsm, stunFsm, wrapper, this),
            new Slash3Modifier(fsm, stunFsm, wrapper, this),
            new Slash6Modifier(fsm, stunFsm, wrapper, this),
            new Slash9Modifier(fsm, stunFsm, wrapper, this),
            new CycloneAnticTransitionerState(fsm, stunFsm, wrapper, this),
            new CycloneAnticModifier(fsm, stunFsm, wrapper, this),
            new Cyclone4Modifier(fsm, stunFsm, wrapper, this),
            new JumpLaunchModifier(fsm, stunFsm, wrapper, this),
            new SpinAttackLandModifier(fsm, stunFsm, wrapper, this),
            new DashGrindTransitionerState(fsm, stunFsm, wrapper, this),
            new DashGrindModifier(fsm, stunFsm, wrapper, this),
            new Phase3RecoveringState(fsm, stunFsm, wrapper, this),
            new Phase3KnockedState(fsm, stunFsm, wrapper, this),
            new TeleportFinalState(fsm, stunFsm, wrapper, this),
            new TripleTeleportSlash3State(fsm, stunFsm, wrapper, this),
            new Teleport3State(fsm, stunFsm, wrapper, this),
            new TripleTeleportSlash2State(fsm, stunFsm, wrapper, this),
            new Teleport2State(fsm, stunFsm, wrapper, this),
            new TripleTeleportSlash1State(fsm, stunFsm, wrapper, this),
            new Teleport1State(fsm, stunFsm, wrapper, this),
        ];
        foreach (var modifier in stateModifiers)
        {
            modifier?.OnCreateModifier();
            stateModifierCollection.Add(modifier!.BindState, modifier);
        }
        ApplyPhase1Modifiers();
    }
    
    private void RerouteFirstRoarState()
    {
        FsmState challengePauseState = fsm.Fsm.GetState("Challenge Pause");
        FsmState jumpInAnticState = fsm.Fsm.GetState("Launch In Antic");
        foreach (var transition in challengePauseState.Transitions)
        {
            if (transition.ToState == "Battle Roar Antic")
            {
                transition.ToFsmState = jumpInAnticState;
                transition.ToState = jumpInAnticState.Name;
            }
        }
    }
    
    private void SubscribeStateChangedEvent() => fsm.Fsm.StateChanged += OnStateChanged;

    private void OnStateChanged(FsmState state)
    {
        CheckStunState(state);
        CheckPhase2State(state);
        CheckPhase3State(state);
    }
    
    private void CheckStunState(FsmState state)
    {
        if (!state.Name.Contains("Stun")) return;
        InstantGetOutOfStunCheck();
    }

    private void CheckPhase2State(FsmState state)
    {
        if (state.Name != "Set P2 Roar") return;
        wrapper.SetPhaseIndex(1);
        ApplyPhase2Modifiers();
    }

    private void CheckPhase3State(FsmState state)
    {
        if (state.Name != "Phase 3 Knocked") return;
        wrapper.SetPhaseIndex(2);
        ApplyPhase3Modifiers();
    }
    
    private void InstantGetOutOfStunCheck()
    {
        if (wrapper.PhaseIndex == 2)
        {
            fsm.SendEvent("FINISHED");
        }
    }
    
    private void ApplyPhase1Modifiers()
    {
        foreach (var modifier in stateModifiers)
        {
            modifier?.SetupPhase1Modifiers();
        }
        main.Log("PHASE 1 MODIFIERS APPLIED");
    }
    
    private void ApplyPhase2Modifiers()
    {
        foreach (var modifier in stateModifiers)
        {
            modifier?.SetupPhase2Modifiers();
        }
        main.Log("PHASE 2 MODIFIERS APPLIED");
    }
    
    private void ApplyPhase3Modifiers()
    {
        foreach (var modifier in stateModifiers)
        {
            modifier?.SetupPhase3Modifiers();
        }
        main.Log("PHASE 3 MODIFIERS APPLIED");
    }

    public float GetStateStartTime()
    {
        return !stateModifierCollection.TryGetValue(fsm.ActiveStateName, out var value) ? 0f : value.AnimationStartTime;
    }

    public void DoPhase3()
    {
        if (fsm.Fsm.GetFsmBool("Phase 3").Value || fsm.ActiveStateName == "BG Dance") return;
        if (!fsm.Fsm.GetFsmBool("Phase 2").Value)
            ApplyPhase2Modifiers();
        wrapper.DoHighlightEffects();
        fsm.SetState("Phase 3 Knocked");    
    }
}