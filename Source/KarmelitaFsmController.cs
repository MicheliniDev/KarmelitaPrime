using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class KarmelitaFsmController(PlayMakerFSM fsm, PlayMakerFSM stunFsm, KarmelitaWrapper wrapper)
{
    private PlayMakerFSM fsm = fsm;
    private PlayMakerFSM stunFsm = stunFsm;
    private KarmelitaWrapper wrapper = wrapper;
    
    public void SubscribeStateChangedEvent() => fsm.Fsm.StateChanged += OnStateChanged;
    public void UnsubscribeStateChangedEvent() => fsm.Fsm.StateChanged -= OnStateChanged;
    
    public void OnStateChanged(FsmState state)
    {
        CheckStunState(state);
        CheckPhase2State(state);
        CheckPhase3State(state);
    }
    
    public void RerouteFirstRoarState()
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
    
    private void CheckStunState(FsmState state)
    {
        if (!state.Name.Contains("Stun")) return;
        KarmelitaPrimeMain.Instance.Log("STUNNED");
        InstantGetOutOfStunCheck();
    }

    private void CheckPhase2State(FsmState state)
    {
        if (state.Name != "Set P2 Roar") return;
        wrapper.SetPhaseIndex(1);
    }

    private void CheckPhase3State(FsmState state)
    {
        if (state.Name != "Set P3 Roar") return;
        wrapper.SetPhaseIndex(2);
    }
    
    private void InstantGetOutOfStunCheck()
    {
        if (wrapper.PhaseIndex == 2)
        {
            fsm.SendEvent("FINISHED");
        }
    }
}