using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class WindBladeState(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Wind Blade";
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
                    ClipName = "Slash 1",
                    AnimationFinishedEvent = FsmEvent.GetFsmEvent("FINISHED"),
                    //shortenEventTIme = 0.1f POSSIBLY CHANGE TO WAIT
                },
                new GetFromPreloadManagerAction()
                {
                    PrefabName = "Song Knight Projectile",
                    SpawnPosition = fsm.Fsm.GetFsmGameObject("Throw Point").Value.transform,
                    GetDelay = 0.15f
                },
            ],
        };
        fsm.Fsm.States = fsm.Fsm.States.Append(bindState).ToArray();
    }

    public override void SetupPhase1Modifiers()
    {
    }

    public override void SetupPhase2Modifiers()
    {
        BindFsmState.Transitions =
        [
            new FsmTransition()
            {
                FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                ToState = "Long Approach",
                ToFsmState = fsm.Fsm.GetState("Long Approach")
            }
        ];
    }

    public override void SetupPhase3Modifiers()
    {
        BindFsmState.Transitions =
        [
            new FsmTransition()
            {
                FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                ToState = "Start Idle",
                ToFsmState = fsm.Fsm.GetState("Start Idle")
            }
        ];
    }
}