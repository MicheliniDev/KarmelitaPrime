using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace KarmelitaPrime;

public class Rethrow3ThrowState(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Rethrow 3 Throw";
    private GameObject spawnedSoTheConsoleWillShutUp;
    public override void OnCreateModifier()
    {
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
        var bindState = new FsmState(fsm.Fsm)
        {
            Name = BindState,
            Actions = 
            [
                new SpawnObjectFromGlobalPool()
                {
                    gameObject = wrapper.SicklePrefab,
                    spawnPoint = fsm.Fsm.GetFsmGameObject("Throw Point"),
                    position = Vector3.zero,
                    rotation = Vector3.one,
                    storeObject = spawnedSoTheConsoleWillShutUp
                },
                new AnimEndSendRandomEventAction()
                {
                    animator = wrapper.animator,
                    events = [FsmEvent.GetFsmEvent("THROW"), FsmEvent.GetFsmEvent("FINISHED")],
                    weights = [.5f, .5f],
                    shortenEventTIme = 0f
                }
            ],
            Transitions = 
            [
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                    ToState = "Counter Attack",
                    ToFsmState = fsm.Fsm.GetState("Counter Attack")
                },
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("THROW"),
                    ToState = "Throw Dir",
                    ToFsmState = fsm.Fsm.GetState("Throw Dir")
                }
            ]
        };
        fsm.Fsm.States = fsm.Fsm.States.Append(bindState).ToArray();
    }
}