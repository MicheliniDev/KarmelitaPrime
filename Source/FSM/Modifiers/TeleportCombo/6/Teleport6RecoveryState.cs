using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class Teleport6RecoveryState(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Teleport 6 Recovery";
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
                new AnimationPlayerAction()
                {
                    animator = wrapper.animator,
                    ClipName = "Jump Attack Land", 
                    AnimationFinishedEvent = FsmEvent.GetFsmEvent("FINISHED"),
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
            ],
            Transitions = [
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                    ToState = "Teleport 6 Wind Slash",
                    ToFsmState = fsm.Fsm.GetState("Teleport 6 Wind Slash")
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