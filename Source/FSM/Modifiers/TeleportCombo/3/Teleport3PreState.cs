using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class Teleport3PreState(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Teleport 3 Pre";
    public override void OnCreateModifier()
    {
        FsmState bindState = new FsmState(fsm.Fsm)
        {
            Name = BindState,
            Actions = 
            [
                new FaceHeroAction()
                {
                    Transform = wrapper.transform,
                },
                new FadeVelocityAction()
                {
                    Rb = wrapper.rb,
                    Duration = 0.1f
                },
                new AnimationPlayerAction()
                {
                    animator = wrapper.animator,
                    ClipName = "Spin Attack Antic", 
                },
                new PlayClipAction()
                {
                    Source = fsm.Fsm.GetFsmGameObject("Audio Loop Voice").Value,
                    Clip = wrapper.KarmelitaTeleportAudio
                },
                new SpawnPrefabAction()
                {
                    Prefab = wrapper.KarmelitaTeleportEffect,
                    Transform = wrapper.transform,
                },
                new Wait()
                {
                    time = 0.2f,
                    finishEvent = FsmEvent.GetFsmEvent("FINISHED")
                },
            ],
            Transitions = [
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                    ToState = "Teleport 3",
                    ToFsmState = fsm.Fsm.GetState("Teleport 3")
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