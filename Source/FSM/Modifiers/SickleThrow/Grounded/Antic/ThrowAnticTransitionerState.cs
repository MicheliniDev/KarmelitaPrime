using System.Linq;
using HutongGames.PlayMaker;

namespace KarmelitaPrime;

public class ThrowAnticTransitionerState(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Throw Antic Transitioner";
    public override void OnCreateModifier()
    {
        var bindState = new FsmState(fsm.Fsm)
        {
            Name = BindState,
            Actions = 
            [
                new AnimEndSendRandomEventAction()
                {
                    animator = wrapper.animator,
                    events = [FsmEvent.GetFsmEvent("FINISHED"), FsmEvent.GetFsmEvent("CANCEL")],
                    weights = [.5f, .5f],
                    shortenEventTIme = 0.55f
                }
            ],
            Transitions = [
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                    ToState = "Throw Dir",
                    ToFsmState = fsm.Fsm.GetState("Throw Dir")
                },
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("CANCEL"),
                    ToState = "Jump Antic", //MAYBE REMOVE THIS?
                    ToFsmState = fsm.Fsm.GetState("Jump Antic")
                },
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("ATTACK"),
                    ToState = "Generic Teleport Pre", 
                    ToFsmState = fsm.Fsm.GetState("Generic Teleport Pre")
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
        var anim = BindFsmState.Actions.FirstOrDefault(action => action is AnimEndSendRandomEventAction) as AnimEndSendRandomEventAction;
        anim.events = [FsmEvent.GetFsmEvent("FINISHED"), FsmEvent.GetFsmEvent("CANCEL"), FsmEvent.GetFsmEvent("ATTACK")];
        anim.weights = [.3f, .3f, .4f];
    }
}