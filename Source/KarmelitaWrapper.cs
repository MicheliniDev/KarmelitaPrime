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
    
    ////fsm.Fsm.StateChanged<FsmState> | EVENT FOR STATE CHANGE, FsmState is the new state that was transitioned into
    private void Awake()
    {
        GetComponents();
        ChangeHealth();
        ChangeTextures();
        RerouteFirstRoarState();
        SetVocalAudioSource(false);
    }
    
    private void GetComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<HealthManager>();
        sprite = GetComponent<tk2dSprite>();
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
    
    private void SetVocalAudioSource(bool active) => vocalSource.gameObject.SetActive(active);
    
    public bool ShouldDealContactDamage()
    {
        return fsm.ActiveStateName.Contains("Slash") || 
               fsm.ActiveStateName.Contains("Dash Grind") || 
               fsm.ActiveStateName.Contains("Spin Attack") || 
               fsm.ActiveStateName.Contains("Cyclone");
    }

    private void OnDestroy()
    {
        SetVocalAudioSource(true);
    }
}