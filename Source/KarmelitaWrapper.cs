using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using TeamCherry.Localization;
using UnityEngine;
using UnityEngine.SceneManagement;
using static KarmelitaPrime.Constants;

namespace KarmelitaPrime;

public class KarmelitaWrapper : MonoBehaviour
{
    private PlayMakerFSM fsm;
    private PlayMakerFSM stunFsm;
    private Rigidbody2D rb;
    private tk2dSprite sprite;
    private AudioSource vocalSource;
    public tk2dSpriteAnimator animator;

    private HealthManager health;
    private KarmelitaFsmController fsmController;

    private Dictionary<string, float> animationSpeedCollection;

    private readonly string[] statesToCancelContactDamage =
    [
        "Idle",
        "Movement",
        "Stun",
        "Evade",
        "Dash"
    ];

    public int PhaseIndex;
    
    // ReSharper disable once IteratorMethodResultIsIgnored
    private void Awake()
    {
        GetComponents();
        ChangeHealth();
        SetupFsmController();
        ChangeTextures();
        SetVocalAudioSource(false);
        SetPhaseIndex(0);
        InitializeAnimationSpeedModifiers();
        PreloadManager.Initialize();
    }

    private void GetComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<HealthManager>();
        sprite = GetComponent<tk2dSprite>();
        animator = GetComponent<tk2dSpriteAnimator>();
        fsm = gameObject.LocateMyFSM("Control");
        stunFsm = gameObject.LocateMyFSM("Stun Control");
        vocalSource = AudioManager.Instance.MusicSources[3];
    }

    private void ChangeHealth()
    {
        HealthChanger.Initialize(health, KarmelitaMaxHp, (int)KarmelitaPhase2HpThreshold, 
            (int)KarmelitaPhase3HpThreshold);
    }

    private void SetupFsmController()
    {
        fsmController = new KarmelitaFsmController(fsm, stunFsm, this);
        fsmController.Initialize();
    }

    public void ChangeTextures()
    {
        var collection = sprite.Collection;
        collection.materials[0].mainTexture = KarmelitaPrimeMain.Instance.KarmelitaTextures[0];
        collection.materials[1].mainTexture = KarmelitaPrimeMain.Instance.KarmelitaTextures[1];
    }

    public void SetPhaseIndex(int index)
    {
        PhaseIndex = index;
        switch (index)
        {
            case 1:
                KarmelitaPrimeMain.Instance.Log("CHANGED TO PHASE 2");
                SetVocalAudioSource(true);
                vocalSource.Play();
                RemoveVocalStopStun();
                break;
            case 2:
                KarmelitaPrimeMain.Instance.Log("CHANGED TO PHASE 3");
                RemoveDazedEffect();
                break;
        }
    }

    private void InitializeAnimationSpeedModifiers()
    {
        animationSpeedCollection = new Dictionary<string, float>()
        {
            {"Slash Antic", SlashAnticSpeed}, //SLASH START
            {"Slash 1", Slash1Speed}, //FIRST TWO SLASHES
            {"Slash 2", Slash2Speed},
            {"Slash End", SlashEndSpeed},
            {"Spin Attack Antic", SpinAttackAnticSpeed}, //SPIN ATTACK
            {"Spin Attack Recoil", SpinAttackRecoilSpeed},
            {"Throw", ThrowSpeed}, //SICKLE THROW GROUND
            {"Throw Antic", ThrowAnticSpeed}, //SICKLE THROW AIR
            {"Air Throw", AirThrowSpeed},
            {"Air Rethrow", AirRethrowSpeed},
            {"Rethrow Antic 1", RethrowAntic1Speed},
            {"Rethrow Antic 2", RethrowAntic2Speed},
            {"Launch Antic", LaunchAnticSpeed}, //SCREW ATTACK
            {"Launch", LaunchSpeed},
            {"Jump Antic", JumpAnticSpeed},
            {"Jump", JumpSpeed}, 
            {"JumpSpin Antic", JumpSpinAnticSpeed}, 
            {"JumpSpin", JumpSpinSpeed}, 
            {"Jump Attack Land", JumpAttackLandSpeed},   
            {"Wall Land", WallLandSpeed}, //DASH GRIND  
            {"Wall Dive", WallDiveSpeed},  
            {"Dash Grind", DashGrindSpeed},  
            {"Dash Grind Spin", DashGrindSpinSpeed},  
        };
    }
    
    private void RemoveDazedEffect()
    {
        var dazeState = stunFsm.FsmStates.FirstOrDefault(state => state.Name == "Dazed Effect");
        FsmStateAction actionToRemove = dazeState!.Actions.FirstOrDefault(action => action is SpawnObjectFromGlobalPool);
        List<FsmStateAction> actions = dazeState.Actions.ToList();
        actions.Remove(actionToRemove);
        dazeState.Actions = actions.ToArray();
    }

    private void RemoveVocalStopStun()
    {
        var actionsList = fsm.Fsm.GetState("Stun Start").Actions.ToList();
        var audioStopAction = actionsList.FirstOrDefault(action => action is TransitionToAudioSnapshot);
        actionsList.Remove(audioStopAction);
        fsm.Fsm.GetState("Stun Start").Actions = actionsList.ToArray();
    }
    
    private void SetVocalAudioSource(bool active) => vocalSource.gameObject.SetActive(active);
    
    public float GetAnimationSpeedModifier(string clip) => animationSpeedCollection.GetValueOrDefault(clip, 1f);
    public float GetAnimationStartTime() => fsmController.GetStateStartTime();
    
    public bool ShouldDealContactDamage() => !statesToCancelContactDamage.Any(state => fsm.ActiveStateName.Contains(state));

    private void OnDestroy()
    {
        SetVocalAudioSource(true);
    }
}