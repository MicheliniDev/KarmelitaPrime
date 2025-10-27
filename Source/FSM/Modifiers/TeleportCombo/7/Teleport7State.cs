using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace KarmelitaPrime;

public class Teleport7State(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Teleport 7";
    public override void OnCreateModifier()
    {
        FsmState bindState = new FsmState(fsm.Fsm)
        {
            Name = BindState,
            Actions = 
            [
                new FaceHeroAction()
                {
                    Transform = wrapper.transform
                },
                new TeleportAction()
                {
                    Target = HeroController.instance.transform,
                    Base = wrapper.transform,
                    OverrideDirection = -(Vector2)wrapper.transform.localScale.normalized,
                    AllowY = false,
                    MinX = 135f,
                    MaxX = 163f,
                    MaxAttempts = 5,
                    MinTeleportDistance = 23f,
                    MaxTeleportDistance = 24f
                },
                new Wait()
                {
                    time = 0.01f,
                    finishEvent = FsmEvent.GetFsmEvent("FINISHED")
                }
            ],
            Transitions = [
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                    ToState = "Teleport 7 Recovery",
                    ToFsmState = fsm.Fsm.GetState("Teleport 7 Recovery")
                },
            ]
        };
        fsm.Fsm.States = fsm.Fsm.States.Append(bindState).ToArray();
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