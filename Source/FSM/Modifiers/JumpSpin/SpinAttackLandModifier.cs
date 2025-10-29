using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class SpinAttackLandModifier(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Spin Attack Land";
    public override void OnCreateModifier()
    {
    }

    public override void SetupPhase1Modifiers()
    {
        var actionsList = BindFsmState.Actions.ToList();
        var anim = BindFsmState.Actions.FirstOrDefault(action => action is Tk2dPlayAnimationWithEvents);
        actionsList.Remove(anim);
        actionsList.AddRange([
            new AnimationPlayerAction()
            {
                animator = wrapper.animator,
                ClipName = "Jump Attack Land",
            },
            new AnimEndSendRandomEventAction()
            {
                animator = wrapper.animator,
                events = [FsmEvent.GetFsmEvent("CANCEL"), FsmEvent.GetFsmEvent("CYCLONE SPIN")],
                weights = [.35f, .65f]
            }
        ]);
        BindFsmState.Actions = actionsList.ToArray();
        BindFsmState.Transitions = 
        [
            new FsmTransition()
            {
                FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                ToState = "Generic Teleport Pre",
                ToFsmState = fsm.Fsm.GetState("Generic Teleport Pre")
            },
            new FsmTransition()
            {
                FsmEvent = FsmEvent.GetFsmEvent("CANCEL"),
                ToState = "Launch Up",
                ToFsmState = fsm.Fsm.GetState("Launch Up")
            },
            new FsmTransition()
            {
                FsmEvent = FsmEvent.GetFsmEvent("CYCLONE SPIN"),
                ToState = "Cyclone Antic",
                ToFsmState = fsm.Fsm.GetState("Cyclone Antic")
            },
            new FsmTransition()
            {
                FsmEvent = FsmEvent.GetFsmEvent("EVADE"),
                ToState = "Evade To Throw",
                ToFsmState = fsm.Fsm.GetState("Evade To Throw")
            }
        ];
    }

    public override void SetupPhase2Modifiers()
    {
        var weightEvent = BindFsmState.Actions.FirstOrDefault(
                action => action is AnimEndSendRandomEventAction) as
            AnimEndSendRandomEventAction;
        weightEvent.events = [FsmEvent.GetFsmEvent("CANCEL"), FsmEvent.GetFsmEvent("CYCLONE SPIN"), FsmEvent.GetFsmEvent("EVADE")];
        weightEvent.weights = [.35f, .35f, .3f];
    }

    public override void SetupPhase3Modifiers()
    {
        var weightEvent = BindFsmState.Actions.FirstOrDefault(
                action => action is AnimEndSendRandomEventAction) as
                AnimEndSendRandomEventAction;
        weightEvent.events = [FsmEvent.GetFsmEvent("CANCEL"), FsmEvent.GetFsmEvent("FINISHED"), FsmEvent.GetFsmEvent("CYCLONE SPIN"), FsmEvent.GetFsmEvent("EVADE")];
        weightEvent.weights = [.25f, .25f, .25f, .25f];
    }
}