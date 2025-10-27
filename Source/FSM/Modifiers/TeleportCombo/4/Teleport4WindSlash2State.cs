using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class Teleport4WindSlash2State(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Teleport 4 Wind Slash 2";
    public override void OnCreateModifier()
    {
        FsmState bindState = new FsmState(fsm.Fsm)
        {
            Name = BindState,
            Actions = 
            [
                new AnimationPlayerAction()
                {
                    animator = wrapper.animator,
                    ClipName = "Slash 2",
                },
                new Wait()
                {
                    time = 0.35f,
                    finishEvent = FsmEvent.GetFsmEvent("FINISHED")
                },
                new GetFromPreloadManagerAction()
                {
                    PrefabName = "Song Knight Projectile",
                    SpawnPosition = fsm.Fsm.GetFsmGameObject("Throw Point").Value.transform,
                    GetDelay = 0.15f
                },
            ],
            Transitions = [
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                    ToState = "Teleport 4 Wind Slash 3",
                    ToFsmState = fsm.Fsm.GetState("Teleport 4 Wind Slash 3")
                }
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