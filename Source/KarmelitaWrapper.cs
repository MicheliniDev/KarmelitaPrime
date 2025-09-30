using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HutongGames.PlayMaker;
using HutongGames;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace KarmelitaPrime;

public class KarmelitaWrapper : MonoBehaviour
{
    private PlayMakerFSM fsm;
    private HealthManager health;
    private Rigidbody2D rb;
    private tk2dSprite sprite;
    private AudioSource vocalSource;
    private StateModifierController modifierController;
    public tk2dSpriteAnimator animator;
    
    private string[] statesToDealContactDamage = new[]
    {
        "Slash",
        "Dash Grind",
        "Spin Attack",
        "Cyclone"
    };

    private int phaseIndex;
    
    private void Awake()
    {
        GetComponents();
        ChangeHealth();
        ChangeTextures();
        RerouteFirstRoarState();
        InitializeStateModifiers();
        SetVocalAudioSource(false);
        phaseIndex = 0;
    }

    private void OnEnable() => SetupEventListeners();
    private void OnDisable() => RemoveEventListeners();
    
    private void GetComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<HealthManager>();
        sprite = GetComponent<tk2dSprite>();
        animator = GetComponent<tk2dSpriteAnimator>();
        fsm = gameObject.LocateMyFSM("Control");
        vocalSource = AudioManager.Instance.MusicSources[3];
    }

    private void ChangeHealth() => HealthChanger.Initialize(health, Constants.KarmelitaMaxHp);
    
    public void ChangeTextures() {
        var collection = sprite.Collection;
        collection.materials[0].mainTexture = KarmelitaPrimeMain.Instance.KarmelitaTextures[0];
        collection.materials[1].mainTexture = KarmelitaPrimeMain.Instance.KarmelitaTextures[1];
    }

    private void RerouteFirstRoarState()
    {
        FsmState challengePauseState = fsm.Fsm.GetState("Challenge Pause");
        FsmState jumpInAnticState = fsm.Fsm.GetState("Launch In Antic");
        for (int i = 0; i < challengePauseState.Transitions.Length; i++)
        {
            if (challengePauseState.Transitions[i].ToState == "Battle Roar Antic")
            {
                challengePauseState.Transitions[i].ToFsmState = jumpInAnticState;
                challengePauseState.Transitions[i].ToState = jumpInAnticState.Name;
            }    
        }
    }

    private void InitializeStateModifiers() => modifierController = StateModifierController.Initialize(animator, fsm);
    
    private void SetVocalAudioSource(bool active)
    {
        vocalSource.gameObject.SetActive(active);
        if (active)
            vocalSource.Play();
    }
    
    private void SetupEventListeners()
    {
        fsm.Fsm.StateChanged += CheckStunState;
        fsm.Fsm.StateChanged += CheckPhase2State;
        fsm.Fsm.StateChanged += CheckPhase3State;
        fsm.Fsm.StateChanged += (FsmState state) =>
        {
            modifierController.ApplyStateModifier(state.Name, phaseIndex);
        };
    }

    private void RemoveEventListeners()
    {
        fsm.Fsm.StateChanged -= CheckStunState;
        fsm.Fsm.StateChanged -= CheckPhase2State;
        fsm.Fsm.StateChanged -= CheckPhase3State;
    }

    private void CheckStunState(FsmState state)
    {
        if (!state.Name.Contains("Stun")) return;
        KarmelitaPrimeMain.Instance.Log("STUNNED");
    }
    
    private void CheckPhase2State(FsmState state)
    {
        if (state.Name != "Set P2 Roar") return;

        KarmelitaPrimeMain.Instance.Log("CHANGED TO PHASE 2");
        SetVocalAudioSource(true);
        phaseIndex = 1;
    }
    
    private void CheckPhase3State(FsmState state)
    {
        if (state.Name != "Set P3 Roar") return;
        
        KarmelitaPrimeMain.Instance.Log("CHANGED TO PHASE 3");
        phaseIndex = 2;
    }
    
    private void OnDestroy()
    {
        SetVocalAudioSource(true);
    }

    public bool ShouldDealContactDamage()
    {
        return statesToDealContactDamage.Any(state => fsm.ActiveStateName.Contains(state));
    }
}