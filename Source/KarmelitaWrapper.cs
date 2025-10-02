using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace KarmelitaPrime;

public class KarmelitaWrapper : MonoBehaviour
{
    private PlayMakerFSM fsm;
    private PlayMakerFSM stunFsm;
    private Rigidbody2D rb;
    private tk2dSprite sprite;
    private AudioSource vocalSource;
    private tk2dSpriteAnimator animator;

    private HealthManager health;
    private KarmelitaFsmController fsmController;

    private Dictionary<string, float> stateSpeedCollection;

    private readonly string[] statesToDealContactDamage =
    [
        "Slash",
        "Dash Grind",
        "Spin Attack",
        "Cyclone"
    ];

    public int PhaseIndex;

    private void Awake()
    {
        GetComponents();
        ChangeHealth();
        SetupFsmController();
        ChangeTextures();
        SetVocalAudioSource(false);
        SetPhaseIndex(0);
        InitializeStateSpeedModifiers();
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
        HealthChanger.Initialize(health, Constants.KarmelitaMaxHp, (int)Constants.KarmelitaPhase2HpThreshold,
            (int)Constants.KarmelitaPhase3HpThreshold);
    }

    private void SetupFsmController()
    {
        fsmController = new KarmelitaFsmController(fsm, stunFsm, this);
        fsmController.RerouteFirstRoarState();
        fsmController.SubscribeStateChangedEvent();
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
                break;
            case 2:
                KarmelitaPrimeMain.Instance.Log("CHANGED TO PHASE 3");
                RemoveDazedEffect();
                break;
        }
    }

    private void InitializeStateSpeedModifiers()
    {
        stateSpeedCollection = new Dictionary<string, float>()
        {
            {"Slash 1", Constants.Slash1Speed},
            {"Slash 2", Constants.Slash2Speed},
            {"Slash 3", Constants.Slash3Speed},
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
    
    private void SetVocalAudioSource(bool active) => vocalSource.gameObject.SetActive(active);
    public float GetSpeedModifier() => stateSpeedCollection.GetValueOrDefault(fsm.ActiveStateName, 1f);
    public bool ShouldDealContactDamage() => statesToDealContactDamage.Any(state => fsm.ActiveStateName.Contains(state));

    private void OnDestroy()
    {
        SetVocalAudioSource(true);
        fsmController.UnsubscribeStateChangedEvent();
    }
}