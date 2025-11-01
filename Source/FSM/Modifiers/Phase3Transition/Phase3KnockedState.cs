using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace KarmelitaPrime;

public class Phase3KnockedState(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Phase 3 Knocked";

    private GameObject finalHitObject;
    private AudioClip karmelitaDeathAudio => wrapper.KarmelitaDeathAudio;
    private AudioClip bossDeathAudio => wrapper.BossGenericDeathAudio;
    public override void OnCreateModifier()
    {
        foreach (var gameObject in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (!gameObject.name.Contains("Boss Death FinalHit")) continue;
            finalHitObject = gameObject;
            break;
        }
        CreateBindState();
    }

    public override void SetupPhase1Modifiers()
    {
    }

    public override void SetupPhase2Modifiers()
    {
    }

    public override void SetupPhase3Modifiers()
    {
    }

    private void CreateBindState()
    {
        var bindFsmState = new FsmState(fsm.Fsm)
        {
            Name = "Phase 3 Knocked",
            Actions = [
                //BASE
                new CheckYVelocityAction()
                {
                    Rb = wrapper.rb,
                    Velocity = 0.01f,
                    OnVelocityMatch = FsmEvent.GetFsmEvent("FINISHED")
                },
                new SetAllMusicSources()
                {
                    Active = false,
                },
                new AnimationPlayerAction()
                {
                    animator = wrapper.animator,
                    ClipName = "Stun Air"
                },
                new SetVelocityToPlayer()
                {
                    Rb = wrapper.rb,
                    velocity = -10f,
                    velocityY = 23f,
                    ResetOnUpdate = true
                },
                new DecelerateXY()
                {
                    decelerationX = 0.9f,
                    decelerationY = 0.9f,
                },
                //FAKE BOSS DEATH
                new PlayClipAction()
                {
                    Clip = karmelitaDeathAudio,
                    Source = fsm.Fsm.GetFsmGameObject("Audio Loop Voice").Value
                },
                new PlayClipAction()
                {
                    Clip = bossDeathAudio,
                    Source = fsm.Fsm.GetFsmGameObject("Audio Loop Voice").Value
                },
                new SpawnPrefabAction()
                {
                    Prefab = finalHitObject,
                },
            ],
            Transitions = [new FsmTransition()
            {
                FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                ToState = "Phase 3 Recovering State",
                ToFsmState = fsm.Fsm.GetState("Phase 3 Recovering State")
            }]
        };
        fsm.Fsm.States = fsm.FsmStates.Append(bindFsmState).ToArray();
    }
}