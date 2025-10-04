using System.Collections.Generic;
using System.Linq;
using GenericVariableExtension;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class KarmelitaFsmController(PlayMakerFSM fsm, PlayMakerFSM stunFsm, KarmelitaWrapper wrapper)
{
    private KarmelitaPrimeMain main => KarmelitaPrimeMain.Instance;
    
    private PlayMakerFSM fsm = fsm;
    private PlayMakerFSM stunFsm = stunFsm;
    private KarmelitaWrapper wrapper = wrapper;
    
    private List<StateModifierBase> stateModifiers;
    
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
            new Slash3Modifier(fsm, stunFsm, wrapper, this)
        ];
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
        if (state.Name != "Set P3 Roar") return;
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
}