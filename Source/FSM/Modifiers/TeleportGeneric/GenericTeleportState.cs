using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class GenericTeleportState(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Generic Teleport";
    public override void OnCreateModifier()
    {
        FsmState bindState = new FsmState(fsm.Fsm)
        {
            Name = BindState,
            Actions = 
            [
                new FadeVelocityAction()
                {
                    Rb = wrapper.rb,
                    Duration = 0.01f,
                },
                new FaceHeroAction()
                {
                    Transform = wrapper.transform
                },
                new TeleportAction()
                {
                    Target = HeroController.instance.transform,
                    Base = wrapper.transform,
                    TeleportToFacing = true,
                    OffsetY = -15f,
                    MinX = 135f,
                    MaxX = 163f,
                    MaxAttempts = 5,
                    MinTeleportDistance = 15f,
                    MaxTeleportDistance = 16f
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
                    ToState = "Generic Teleport Recovery",
                    ToFsmState = fsm.Fsm.GetState("Generic Teleport Recovery")
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