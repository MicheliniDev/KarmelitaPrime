using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class Teleport1State(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Teleport 1";
    public override void OnCreateModifier()
    {
        FsmState bindState = new FsmState(fsm.Fsm)
        {
            Name = BindState,
            Actions = [new SendEventByName()
            {
                sendEvent = "FINISHED",
            },
            new SetGravity2dScale()
            {
                gameObject = new FsmOwnerDefault()
                {
                    GameObject = wrapper.gameObject
                },
                gravityScale = 0f,
            }],
        };
        fsm.Fsm.States = fsm.Fsm.States.Append(bindState).ToArray();

        BindFsmState.Transitions = [
            new FsmTransition()
            {
                FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                ToState = "Triple Teleport Slash 1",
                ToFsmState = fsm.Fsm.GetState("Triple Teleport Slash 1")
            },
        ];
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