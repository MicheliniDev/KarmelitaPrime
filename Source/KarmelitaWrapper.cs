using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using UnityEngine.SceneManagement;
using static KarmelitaPrime.Constants;

namespace KarmelitaPrime;

public class KarmelitaWrapper : MonoBehaviour
{
    private PlayMakerFSM fsm;
    private PlayMakerFSM stunFsm;
    private tk2dSprite sprite;
    private AudioSource vocalSource;
    
    public tk2dSpriteAnimator animator;
    public HealthManager health;
    public Rigidbody2D rb;
    public SpriteFlash SpriteFlash;
    public Shader FlashShader;
    public int PhaseIndex;
        
    private KarmelitaFsmController fsmController;
    private Dictionary<string, float> animationSpeedCollection;
    private readonly string[] statesToCancelContactDamage =
    [
        "Start Idle",
        "Movement 1",
        "Movement 2",
        "Movement 3",
        "Movement 4",
        "Movement 5",
        "Evade",
        "Long Evade",
        "Dash",
        "Stun Start",
        "Stun Air",
        "Stun Land",
        "Stunned",
        "Stun Damage",
        "Damage Recover",
        "Stun Recover",
        "Approach Block",
        "Jump Antic",
        "Spin Attack Land",
        "Throw Fall",
        "Throw Land",
        "P2 Roar Antic",
        "Phase 3 Knocked",
        "Phase 3 Recovering State",
        "P2 Roar Antic",
        "P2 Roar",
        "P3 Roar Antic",
        "P3 Roar"
    ];
    private float auraLevel;

    public GameObject BlackScreen;
    public bool IsInHighlightMode;
    public bool hasFakedP3;

    public RandomAudioClipTable StunTable;
    public RandomAudioClipTable AttackQuickTable;
    public RandomAudioClipTable AttackLongTable;
    public AudioClip KarmelitaRoarFinal;
    public AudioClip SwordClip;
    public AudioClip LandClip;
    public AudioClip CycloneClip;
    public AudioClip BossGenericDeathAudio;
    public AudioClip KarmelitaDeathAudio;
    
    public GameObject KarmelitaTeleportEffect;
    public AudioClip KarmelitaTeleportAudio;
    
    public GameObject SicklePrefab;

    public IEnumerator Start()
    {
        yield return StartCoroutine(PreloadManager.LoadAllAssets());
        GetComponents();
        ChangeHealth();
        ChangeTextures();
        SetupFsmController();
        SetVocalAudioSource(false);
        SetPhaseIndex(0);
        InitializeAnimationSpeedModifiers();
        SetupBlackScreen();
        auraLevel = 0f;
        IsInHighlightMode = false;
        hasFakedP3 = false;
        HeroController.instance.OnDeath += () => KarmelitaPrimeMain.Instance.ResetFlags();
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.G))
        {
            var source = fsm.Fsm.GetFsmGameObject("Audio Loop Voice").Value.GetComponent<AudioSource>();
            var audioEvent = new AudioEvent()
            {
                Clip = KarmelitaTeleportAudio,
                PitchMin = 1f,
                PitchMax = 1f,
                Volume = source.volume
            };
            audioEvent.SpawnAndPlayOneShot(transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            var obj = Instantiate(KarmelitaTeleportEffect, transform.position, Quaternion.identity);
            obj.SetActive(true);
        }*/
    }
    
    private void GetComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<HealthManager>();
        sprite = GetComponent<tk2dSprite>();
        animator = GetComponent<tk2dSpriteAnimator>();
        SpriteFlash = GetComponent<SpriteFlash>();
        FlashShader = GetComponent<MeshRenderer>().material.shader;
        fsm = gameObject.LocateMyFSM("Control");
        stunFsm = gameObject.LocateMyFSM("Stun Control");
        vocalSource = AudioManager.Instance.MusicSources[3];
        
        foreach (var audioClip in Resources.FindObjectsOfTypeAll<AudioClip>())
        {
            if (audioClip.name.Contains("Karmelita-Death-Hit-003"))
            {
                KarmelitaDeathAudio = audioClip;
            }
            if (audioClip.name.Contains("boss_death_pt_1"))
            {
                BossGenericDeathAudio = audioClip;
            }
            if (audioClip.name.Contains("ruin_sentry_sword_1"))
            {
                SwordClip = audioClip;
            }
            if (audioClip.name.Contains("carmelita_spin_land"))
            {
                LandClip = audioClip;
            }
            if (audioClip.name.Contains("carmelita_cyclone"))
            {
                CycloneClip = audioClip;
            }
            if (audioClip.name.Contains("bone_hunter_dive"))
            {
                KarmelitaTeleportAudio = audioClip;
            }
            if (audioClip.name.Contains("Karmelita-Roar-002_shortened"))
            {
                KarmelitaRoarFinal = audioClip;
            }   
        }
        
        foreach (var table in Resources.FindObjectsOfTypeAll<RandomAudioClipTable>())
        {
            if (table.name.Contains("Karmelita-Stun"))
            {
                StunTable = table;
            }
            if (table.name.Contains("Karmelita-Attack-Quick"))
            {
                AttackQuickTable = table;
            }
            if (table.name.Contains("Karmelita-Attack-Long"))
            {
                AttackLongTable = table;
            }
        }

        foreach (var prefab in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (prefab.name.Contains("Carmelita Sickle"))
            {
                SicklePrefab = prefab;
            }
            if (prefab.name.Contains("Emerge Effect"))
            {
                KarmelitaTeleportEffect = prefab;
            }
        }
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
        collection.materials[0].mainTexture = KarmelitaPrimeMain.Instance.CurrentTextures[0];
        collection.materials[1].mainTexture = KarmelitaPrimeMain.Instance.CurrentTextures[1];
    }

    public void OnPrefabSpawn(GameObject prefab, Transform spawnPoint)
    {
        var instance = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        OnPrefabSpawn(instance);
    }
    
    public void OnPrefabSpawn(GameObject prefab)
    {
        if (IsInHighlightMode)
        {
            if (!prefab.TryGetComponent<HighlightTracker>(out var tracker))
                tracker = prefab.AddComponent<HighlightTracker>();
            tracker.ApplyHighlightEffect();   
        }
        
        if (prefab.name.Contains("Song Knight Projectile"))
        {
            var instance = prefab;
            
            var Rb = instance.GetComponent<Rigidbody2D>();
            Vector3 scale = instance.transform.localScale;
            Vector3 position = instance.transform.position;
            
            if (HeroController.instance.transform.position.x > instance.transform.position.x && instance.transform.localScale.x > 0 ||
                HeroController.instance.transform.position.x < instance.transform.position.x && instance.transform.localScale.x < 0)
                scale.x *= -1f;

            scale.x *= 4f;
            scale.y *= 1.5f;
            position.y += 0.4f;
            instance.transform.localScale = scale;
            instance.transform.position = position;
            Rb.linearVelocityX = 60f * -Rb.gameObject.transform.localScale.normalized.x;   
        }
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

    private void SetupBlackScreen()
    {
        BlackScreen = new GameObject("Black Screen", typeof(SpriteRenderer));
        var position = BlackScreen.transform.position;
        position.z = 0.1f;
        position.x = transform.position.x;
        position.y = transform.position.y;
        BlackScreen.transform.position = position;
        BlackScreen.transform.localScale *= 100f;
        SpriteRenderer sr = BlackScreen.GetComponent<SpriteRenderer>();

        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, new Color(0f, 0f, 0f, 1f));
        tex.Apply();

        Sprite blackSprite = Sprite.Create(
            tex,
            new Rect(0, 0, 1, 1),
            new Vector2(0.5f, 0.5f),
            1f 
        );

        sr.sprite = blackSprite;
        sr.color = new Color(0f, 0f, 0f, 0f);
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

    public void DoHighlightEffects()
    {
        vocalSource.gameObject.SetActive(true);
        
        GameCameras.instance.hudCamera.gameObject.SetActive(false);
        
        transform.Find("HeroLight").gameObject.SetActive(false);
        HeroController.instance.heroLight.Alpha = 0f;
        
        RecolorNonKarmelitaSprites();
        BlackScreen.GetComponent<SpriteRenderer>().color = Color.black;
        
        HeroController.instance.SpriteFlash.Flash(Color.white, 1f, 0f, 9999f, 1f, 1f);
        SpriteFlash.Flash(Color.white, 1f, 0f, 9999f, 1f, 1f);
        
        IsInHighlightMode = true;
    }

    private void RecolorNonKarmelitaSprites()
    {
        var flashShader = FlashShader;
        foreach (var gameobject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (gameobject.TryGetComponent<SpriteRenderer>(out var rootRenderer))
                rootRenderer.color = Color.black;

            foreach (var childRenderer in gameobject.GetComponentsInChildren<Renderer>(true))
            {
                if (childRenderer.name.Contains("Hunter Queen Boss")) continue;
        
                if (childRenderer.name == "spear")
                {
                    var material = new Material(flashShader);
                    childRenderer.material = material;
                    childRenderer.material.SetFloat(Shader.PropertyToID("_FlashAmount"), 1f);
                    childRenderer.material.SetColor(Shader.PropertyToID("_FlashColor"), Color.white);
                }
                else
                {
                    childRenderer.material.color = Color.black;
                }
            }
        }
    }

    public void FarmAura(float amount, bool instantTrigger = false)
    {
        auraLevel += amount;
        if (instantTrigger)
        {
            StartCoroutine(WaitForForcePhase3(0.4f));
        }
        else if (auraLevel >= 100f)
        {
            TriggerPhase3();
        }    
        
        if (auraLevel >= 1000f)
        {
            health.Die(0, AttackTypes.Generic, false);
        }  
    }

    private IEnumerator WaitForForcePhase3(float duration)
    {
        yield return new WaitForSeconds(duration);
        TriggerPhase3();
        yield return null;
    }

    public void EnableInvincible(bool invincible) => health.IsInvincible = invincible;
    
    private void SetVocalAudioSource(bool active) => vocalSource.gameObject.SetActive(active);
    
    public float GetAnimationSpeedModifier(string clip) => animationSpeedCollection.GetValueOrDefault(clip, 1f);
    public float GetAnimationStartTime() => fsmController.GetStateStartTime();
    public bool ShouldDealContactDamage() => statesToCancelContactDamage.All(state => fsm.ActiveStateName != state);
    
    public void TriggerPhase3() => fsmController.DoPhase3();

    public void FakeP3()
    {
        if (hasFakedP3) return;
        fsmController.FakePhase3();
        hasFakedP3 = true;
    }
    
    public void LoseAura(float amount) => auraLevel -= amount;
    
    private void OnDestroy()
    {
        SetVocalAudioSource(true);
        IsInHighlightMode = false;
        PreloadManager.UnloadAll();
    }
}