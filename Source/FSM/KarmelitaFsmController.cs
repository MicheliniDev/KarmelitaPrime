using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

    public void Initialize()
    {
        RerouteFirstRoarState();
        ChangeToBigTitle();
        SubscribeStateChangedEvent();
        stateModifiers = [
            new CounterAttackState(fsm, stunFsm, wrapper, this),
            new CounterAttackPreState(fsm, stunFsm, wrapper, this),
            new ApproachBlockModifier(fsm, stunFsm, wrapper, this),
            new SlashAnticModifier(fsm, stunFsm, wrapper, this),
            new NewSlash1State(fsm, stunFsm, wrapper, this),
            new NewSlash2State(fsm, stunFsm, wrapper, this),
            new Slash1Modifier(fsm, stunFsm, wrapper, this),
            new Slash3TransitionerState(fsm, stunFsm, wrapper, this),
            new Slash3Modifier(fsm, stunFsm, wrapper, this),
            new Slash4Modifier(fsm, stunFsm, wrapper, this),
            new Slash9Modifier(fsm, stunFsm, wrapper, this),
            new CycloneAnticTransitionerState(fsm, stunFsm, wrapper, this),
            new CycloneAnticModifier(fsm, stunFsm, wrapper, this),
            new Cyclone4Modifier(fsm, stunFsm, wrapper, this),
            new JumpLaunchModifier(fsm, stunFsm, wrapper, this),
            new SpinAttackLandModifier(fsm, stunFsm, wrapper, this),
            new DashGrindTransitionerState(fsm, stunFsm, wrapper, this),
            new DashGrindModifier(fsm, stunFsm, wrapper, this),
            new TeleportFinalState(fsm, stunFsm, wrapper, this),
            new TripleTeleportSlash3State(fsm, stunFsm, wrapper, this),
            new Teleport3State(fsm, stunFsm, wrapper, this),
            new TripleTeleportSlash2State(fsm, stunFsm, wrapper, this),
            new Teleport2State(fsm, stunFsm, wrapper, this),
            new TripleTeleportSlash1State(fsm, stunFsm, wrapper, this),
            new Teleport1State(fsm, stunFsm, wrapper, this),
            new Phase3RecoveringState(fsm, stunFsm, wrapper, this),
            new Phase3KnockedState(fsm, stunFsm, wrapper, this),
            new Rethrow3ThrowState(fsm, stunFsm, wrapper, this),
            new Rethrow3State(fsm, stunFsm, wrapper, this),
            new ThrowAnticTransitionerState(fsm, stunFsm, wrapper, this),
            new ThrowAnticModifier(fsm, stunFsm, wrapper, this),
            new Rethrow2TransitionerState(fsm, stunFsm, wrapper, this),
            new Rethrow2Modifier(fsm, stunFsm, wrapper, this),
        ];
        foreach (var modifier in stateModifiers)
        {
            modifier?.OnCreateModifier();
            stateModifierCollection.Add(modifier!.BindState, modifier);
        }
        ApplyPhase1Modifiers();
        SetIdleTime(0.25f, 0.5f);
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

    private void ChangeToBigTitle()
    {
        var roarState = fsm.Fsm.States.FirstOrDefault(state => state.Name == "Roar");
        var actionsList = roarState!.Actions.ToList();
        var title = roarState.Actions.FirstOrDefault(action => action is DisplayBossTitle);
        var actionToRemove = title;
        actionsList.Remove(actionToRemove);
        roarState.Actions = actionsList.ToArray();
            
        var landState = fsm.Fsm.States.FirstOrDefault(state => state.Name == "Entry Fall");
        landState!.Actions = landState.Actions.Append(new DisplayBossTitle()
        {
            areaTitleObject = GameObject.Find("_GameCameras/HudCamera/In-game/Area Title"),
            displayRight = false,
            bossTitle = "HUNTER_QUEEN_BC"
        }).ToArray();
    }
    
    private void SubscribeStateChangedEvent() => fsm.Fsm.StateChanged += OnStateChanged;

    private void SetIdleTime(float min, float max)
    {
        fsm.Fsm.GetFsmFloat("Idle Min").Value = min;
        fsm.Fsm.GetFsmFloat("Idle Max").Value = max;
    }
    
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
        if (state.Name != "P2 Roar Antic") return;
        wrapper.SetPhaseIndex(1);
        ApplyPhase2Modifiers();
        SetIdleTime(0.20f, 0.25f);
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

    public void FakePhase3()
    {
        var bindState = new FsmState(fsm.Fsm)
        {
            Name = "Fake Phase 3",
            Transitions = 
            [
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                    ToState = "Set Dash Grind",
                    ToFsmState = fsm.Fsm.GetState("Set Dash Grind")
                }
            ]
        };
        CloneActions(fsm.Fsm.GetState("P3 Roar Antic"), bindState);
        var actionsList = bindState.Actions.ToList();
        var actionToRemove = actionsList.FirstOrDefault(action => action is Tk2dPlayAnimationWithEvents);
        actionsList.Remove(actionToRemove);
        actionsList.AddRange(
        [
            new AnimationPlayerAction()
            {
                animator = wrapper.animator,
                ClipName = "Roar",
            },
            new StartRoarEmitter()
            {
                spawnPoint = new FsmOwnerDefault()
                {
                    OwnerOption = OwnerDefaultOption.UseOwner,
                    GameObject = wrapper.gameObject
                },
                delay = 0f,
                stunHero = false,
                roarBurst = false,
                isSmall = false,
                noVisualEffect = false,
                forceThroughBind = false,
                stopOnExit = true,
            },
            new Wait()
            {
                time = 1.6f,
                finishEvent = FsmEvent.GetFsmEvent("FINISHED")
            },
            new FadeVelocityAction()
            {
                Rb = wrapper.rb,
                Duration = 0.01f
            },
        ]);
        bindState.Actions = actionsList.ToArray();
        fsm.Fsm.States = fsm.Fsm.States.Append(bindState).ToArray();
        
        fsm.SetState("Fake Phase 3");
    }
    
    public void DoPhase3()
    {
        if (fsm.Fsm.GetFsmBool("Phase 3").Value || fsm.ActiveStateName == "BG Dance") return;
        
        if (!fsm.Fsm.GetFsmBool("Phase 2").Value)
            ApplyPhase2Modifiers();
        
        wrapper.DoHighlightEffects();
        fsm.SetState("Phase 3 Knocked");    
    }
    
    private void CloneActions(FsmState source, FsmState target)
    {
        //This is way too useful, why didn't I make it earlier? Am I stupid?
        var originalActions = source.Actions;
        target.Actions = new FsmStateAction[originalActions.Length];
        for (int i = 0; i < originalActions.Length; i++)
        {
            var action = originalActions[i];
            target.Actions[i] = CloneAction(action);
        }
    }

   private FsmStateAction CloneAction(FsmStateAction originalActions)
    {
        var actionsType = originalActions.GetType();
        var actionCopy = (FsmStateAction)Activator.CreateInstance(actionsType);

        var actionFields = actionsType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (var field in actionFields)
        {
            field.SetValue(actionCopy, field.GetValue(originalActions));
        }
        return actionCopy;
    }
}