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
        SetIdleTime();
        RerouteFirstRoarState();
        ChangeToBigTitle();
        SubscribeStateChangedEvent();
        AddModifiers();
        ApplyPhase1Modifiers();
    }

    private void SetIdleTime()
    {
        fsm.Fsm.GetFsmFloat("Idle Min").Value = 0.25f;
        fsm.Fsm.GetFsmFloat("Idle Max").Value = 0.25f;
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
    
    private void OnStateChanged(FsmState state)
    {
        KarmelitaPrimeMain.Instance.Log($"CHANGED TO {state.Name}");
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
        if (fsm.Fsm.GetFsmBool("Phase 3").Value) return;
        
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
        SetTransitionToTeleportCombo();
    }
    
    public void CloneActions(FsmState source, FsmState target)
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

    private void SetTransitionToTeleportCombo()
    {
        var roarState = fsm.Fsm.GetState("P3 Roar");
        roarState.Transitions =
        [
            new FsmTransition()
            {
                FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                ToState = "Teleport 1 Pre",
                ToFsmState = fsm.Fsm.GetState("Teleport 1 Pre")
            }
        ];
    }
    
    private void AddModifiers()
    {
        stateModifiers = [
            // 1. Foundational State Creators //TELEPORT GENERIC | COUNTER ATTACK
            new SpinAttackModifier(fsm, stunFsm, wrapper, this),
            new WindBladeState(fsm, stunFsm, wrapper, this),
            new GenericTeleportRecoveryState(fsm, stunFsm, wrapper, this),
            new GenericTeleportState(fsm, stunFsm, wrapper, this),
            new GenericTeleportPreState(fsm, stunFsm, wrapper, this),
            new CycloneAnticTransitionerState(fsm, stunFsm, wrapper, this),
            new CycloneAnticModifier(fsm, stunFsm, wrapper, this),
            new NewSlash2State(fsm, stunFsm, wrapper, this),
            new CounterAttackState(fsm, stunFsm, wrapper, this),
            new ThrowAnticTransitionerState(fsm, stunFsm, wrapper, this),
            new DashGrindTransitionerState(fsm, stunFsm, wrapper, this),
            new DashGrindModifier(fsm, stunFsm, wrapper, this),
            new Phase3RecoveringState(fsm, stunFsm, wrapper, this),
            new Rethrow2TransitionerState(fsm, stunFsm, wrapper, this),
            new Rethrow3State(fsm, stunFsm, wrapper, this),
            
            //1.1 - P3 Teleport Combo
            //7
            new Teleport7RecoveryState(fsm, stunFsm, wrapper, this),
            new Teleport7State(fsm, stunFsm, wrapper, this),
            new Teleport7PreState(fsm, stunFsm, wrapper, this),
            //6
            new Teleport6WindSlashState(fsm, stunFsm, wrapper, this),
            new Teleport6RecoveryState(fsm, stunFsm, wrapper, this),
            new Teleport6State(fsm, stunFsm, wrapper, this),
            new Teleport6PreState(fsm, stunFsm, wrapper, this),
            //5
            new Teleport5SickleThrow(fsm, stunFsm, wrapper, this),
            new Teleport5SickleThrowPrepareRightState(fsm, stunFsm, wrapper, this),
            new Teleport5SickleThrowPrepareLeftState(fsm, stunFsm, wrapper, this),
            new Teleport5SickleCheckDirectionState(fsm, stunFsm, wrapper, this),
            new Teleport5RecoveryState(fsm, stunFsm, wrapper, this),
            new Teleport5State(fsm, stunFsm, wrapper, this),
            new Teleport5PreState(fsm, stunFsm, wrapper, this),
            //4
            new Teleport4WindSlash3State(fsm, stunFsm, wrapper, this),
            new Teleport4WindSlash2State(fsm, stunFsm, wrapper, this),
            new Teleport4WindSlash1State(fsm, stunFsm, wrapper, this),
            new Teleport4CounterAttackState(fsm, stunFsm, wrapper, this),
            new Teleport4RecoveryState(fsm, stunFsm, wrapper, this),
            new Teleport4State(fsm, stunFsm, wrapper, this),
            new Teleport4PreState(fsm, stunFsm, wrapper, this),
            //3
            new TripleTeleportSlash3State(fsm, stunFsm, wrapper, this),
            new Teleport3RecoveryState(fsm, stunFsm, wrapper, this),
            new Teleport3State(fsm, stunFsm, wrapper, this),
            new Teleport3PreState(fsm, stunFsm, wrapper, this),
            //2
            new TripleTeleportSlash2State(fsm, stunFsm, wrapper, this),
            new Teleport2RecoveryState(fsm, stunFsm, wrapper, this),
            new Teleport2State(fsm, stunFsm, wrapper, this),
            new Teleport2PreState(fsm, stunFsm, wrapper, this),
            //1
            new TripleTeleportSlash1State(fsm, stunFsm, wrapper, this),
            new Teleport1RecoveryState(fsm, stunFsm, wrapper, this),
            new Teleport1State(fsm, stunFsm, wrapper, this),
            new Teleport1PreState(fsm, stunFsm, wrapper, this),

            // 2. First-Level Dependent State Creators
            new CounterAttackPreState(fsm, stunFsm, wrapper, this),
            new NewSlash1State(fsm, stunFsm, wrapper, this),
            new Phase3KnockedState(fsm, stunFsm, wrapper, this),
            new Rethrow3PrepareLeftState(fsm, stunFsm, wrapper, this),
            new Rethrow3PrepareRightState(fsm, stunFsm, wrapper, this),
            new Slash3TransitionerState(fsm, stunFsm, wrapper, this),

            // 3. Second-Level Dependent State Creators
            new Rethrow3CheckDirectionState(fsm, stunFsm, wrapper, this),

            // 4. Dependent State Modifiers 
            new Rethrow2Modifier(fsm, stunFsm, wrapper, this),
            new Slash3Modifier(fsm, stunFsm, wrapper, this),
            new Slash9Modifier(fsm, stunFsm, wrapper, this),
            new SlashAnticModifier(fsm, stunFsm, wrapper, this),
            new ThrowAnticModifier(fsm, stunFsm, wrapper, this),
            new ThrowLandModifier(fsm, stunFsm, wrapper, this),

            // 5. Independent State Modifiers
            new AirRethrowModifier(fsm, stunFsm, wrapper, this),
            new ApproachBlockModifier(fsm, stunFsm, wrapper, this),
            new Cyclone1Modifier(fsm, stunFsm, wrapper, this),
            new Cyclone4Modifier(fsm, stunFsm, wrapper, this),
            new DoubleThrowQuestionModifier(fsm, stunFsm, wrapper, this),
            new JumpLaunchModifier(fsm, stunFsm, wrapper, this),
            new Slash1Modifier(fsm, stunFsm, wrapper, this),
            new Slash4Modifier(fsm, stunFsm, wrapper, this),
            new SpinAttackLandModifier(fsm, stunFsm, wrapper, this)
        ];
        foreach (var modifier in stateModifiers)
        {
            modifier?.OnCreateModifier();
            stateModifierCollection.Add(modifier!.BindState, modifier);
        }
    }
}