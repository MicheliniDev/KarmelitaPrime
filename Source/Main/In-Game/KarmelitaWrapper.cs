using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using UnityEngine.SceneManagement;
using KarmelitaPrime.Managers;
using static KarmelitaPrime.Constants;

namespace KarmelitaPrime;

public class KarmelitaWrapper : MonoBehaviour
{
    private PlayMakerFSM fsm;
    private PlayMakerFSM stunFsm;
    private tk2dSprite sprite;
    private AudioSource vocalSource;
    private KarmelitaFsmController fsmController;

    public SpriteChanger KarmelitaSpriteChanger;
    public tk2dSpriteAnimator animator;
    public HealthManager health;
    public Rigidbody2D rb;
    public SpriteFlash SpriteFlash;
    
    public Shader FlashShader;
    
    public int PhaseIndex;
    private float auraLevel;
    private bool hasFakedP3;
    private bool hasTriggeredP3;
    public bool IsInHighlightMode;
    public bool BattleStarted;

    public GameObject BlackScreen;

    public RandomAudioClipTable AttackQuickTable;
    public RandomAudioClipTable AttackLongTable;
    public AudioClip SwordClip;
    public AudioClip LandClip;
    public AudioClip CycloneClip;
    public AudioClip BossGenericDeathAudio;
    public AudioClip KarmelitaDeathAudio;
    
    public GameObject KarmelitaTeleportEffect;
    public AudioClip KarmelitaTeleportAudio;
    
    public IEnumerator Start()
    {
        yield return StartCoroutine(PreloadManager.LoadAllAssets());
        GetComponents();
        GetResources();
        SetupChangers();
        SetVocalAudioSource(false);
        SetPhaseIndex(0);
        SetupBlackScreen();
        auraLevel = 0f;
        IsInHighlightMode = false;
        hasFakedP3 = false;
        hasTriggeredP3 = false;
        BattleStarted = false;
        HeroController.instance.OnDeath += () => KarmelitaPrimeMain.Instance.ResetFlags();
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
    }

    private void GetResources()
    {
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
        }
        
        foreach (var table in Resources.FindObjectsOfTypeAll<RandomAudioClipTable>())
        {
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
            if (prefab.name.Contains("Emerge Effect"))
            {
                KarmelitaTeleportEffect = prefab;
            }
        }
    }

    private void SetupChangers()
    {
        HealthChanger.Initialize(health, KarmelitaMaxHp, (int)KarmelitaPhase2HpThreshold, 
            (int)KarmelitaPhase3HpThreshold);
        
        fsmController = new KarmelitaFsmController(fsm, stunFsm, this);
        fsmController.Initialize();

        KarmelitaSpriteChanger = SpriteChanger.Initialize(sprite, KarmelitaPrimeMain.Instance.CurrentTextures);
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
        
        //SECOND SENTINEL PROJECTILE
        if (prefab.name.Contains("Song Knight Projectile"))
        {
            var instance = prefab;
            
            var Rb = instance.GetComponent<Rigidbody2D>();
            Vector3 scale = instance.transform.localScale;
            Vector3 position = instance.transform.position;
            
            //FACING PLAYER BUT GOOD NOW :)
            if (HeroController.instance.transform.position.x > transform.position.x && instance.transform.localScale.x > 0 ||
                HeroController.instance.transform.position.x < transform.position.x && instance.transform.localScale.x < 0)
                scale.x *= -1f;

            //SETTING POSITION AND STUFF
            scale.x *= 4f;
            scale.y *= 1.5f;
            position.y += 0.4f;
            instance.transform.localScale = scale;
            instance.transform.position = position;
            Rb.linearVelocityX = 40f * -Rb.gameObject.transform.localScale.normalized.x;   
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
        SetVocalAudioSource(true);
        
        HudGlobalHide.IsHidden = true;
        HeroController.instance.transform.Find("Vignette").gameObject.SetActive(false);
        GameObject.Find("_GameCameras/CameraParent/tk2dCamera/SceneParticlesController").SetActive(false);
        
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
    
    private void SetVocalAudioSource(bool active) => vocalSource.gameObject.SetActive(active);

    public float GetAnimationSpeedModifier(string clip)
    {
        if (fsm.ActiveStateName.Contains("Teleport 4 Wind Slash") || fsm.ActiveStateName.Contains("Wind Blade"))
            return 1f;
        return AnimationSpeedCollection.GetValueOrDefault(clip, 1f);
    }
    public float GetAnimationStartTime() => fsmController.GetStateStartTime();
    public bool ShouldDealContactDamage() => StatesToCancelContactDamage.All(state => fsm.ActiveStateName != state);
    public void LoseAura(float amount) => auraLevel -= amount;

    public void TriggerPhase3()
    {
        if (hasTriggeredP3) return;
        
        var pos = transform.position;
        pos.y = 21.4355f;
        transform.position = pos;
        rb.linearVelocity = Vector2.zero;
        health.IsInvincible = true;
        fsmController.DoPhase3();
        hasTriggeredP3 = true;
    }
    
    public void FakeP3()
    {
        if (hasFakedP3) return;
        var pos = transform.position;
        pos.y = 21.4355f;
        transform.position = pos;
        rb.linearVelocity = Vector2.zero;
        fsmController.FakePhase3();
        hasFakedP3 = true;
    }
    
    private void OnDestroy()
    {
        SetVocalAudioSource(true);
        IsInHighlightMode = false;
        BattleStarted = false;
        hasTriggeredP3 = false;
        PreloadManager.UnloadAll();
    }
}