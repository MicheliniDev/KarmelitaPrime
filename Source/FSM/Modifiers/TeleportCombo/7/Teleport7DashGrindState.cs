using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class Teleport7DashGrindState(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Teleport 7 Dash Grind";
    public override void OnCreateModifier()
    {
        FsmState bindState = new FsmState(fsm.Fsm)
        {
            Name = BindState,
            Transitions = [
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("WALL"),
                    ToState = "Start Idle", 
                    ToFsmState = fsm.Fsm.GetState("Start Idle")
                },
            ]
        };
        fsm.Fsm.States = fsm.Fsm.States.Append(bindState).ToArray();
        fsmController.CloneActions(fsm.Fsm.GetState("Dash Grind"), BindFsmState);
        var vel = BindFsmState.Actions.FirstOrDefault(action => action is SetVelocityByScale) as SetVelocityByScale; ;
        vel.speed.Value *= 1.5f;
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