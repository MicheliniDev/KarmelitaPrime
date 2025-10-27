using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace KarmelitaPrime;

public class Rethrow3State(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Rethrow 3";
    private GameObject spawnedSoTheConsoleWillShutUp;
    public override void OnCreateModifier()
    {
        FsmState bindState = new FsmState(fsm.Fsm)
        {
            Name = BindState,
            /*Actions = 
            [
                new FaceHeroAction()
                {
                    Transform = wrapper.transform
                },
                new SpawnObjectFromGlobalPoolDelay()
                {
                    gameObject = wrapper.SicklePrefab,
                    spawnPoint = fsm.Fsm.GetFsmGameObject("Throw Point"),
                    position = Vector3.zero,
                    rotation = Vector3.one,
                    delayMin = 0.1f,
                    delayMax = 0.1f,
                    storeObject = spawnedSoTheConsoleWillShutUp
                },
                new AnimationPlayerAction()
                {
                    animator = wrapper.animator,
                    ClipName = "Throw", 
                    AnimationFinishedEvent = FsmEvent.GetFsmEvent("FINISHED"),
                },
                new PlayRandomClipAction()
                {
                    Table = wrapper.AttackQuickTable,
                    Source = fsm.Fsm.GetFsmGameObject("Audio Loop Voice").Value
                },
            ],*/
            Transitions = [
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                    ToState = "Generic Teleport Pre",
                    ToFsmState = fsm.Fsm.GetState("Generic Teleport Pre")
                },
            ]
        };
        fsm.Fsm.States = fsm.Fsm.States.Append(bindState).ToArray();
        fsmController.CloneActions(fsm.Fsm.GetState("Throw 1"), BindFsmState);
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
}