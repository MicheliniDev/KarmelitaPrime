using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace KarmelitaPrime;

public class SlashAnticModifier(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Slash Antic";
    public override void OnCreateModifier()
    {
        var watchAnim = BindFsmState.Actions.FirstOrDefault(action => action is Tk2dPlayAnimationWithEvents);
        var list = BindFsmState.Actions.ToList();
        list.Remove(watchAnim);
        list.AddRange
        (
    [
                new FadeVelocityAction()
                {
                    Rb = wrapper.rb,
                    Duration = 0.01f,
                },
                new AnimationPlayerAction()
                {
                    animator = wrapper.animator,
                    ClipName = "Slash Antic"
                },
                new AnimEndSendRandomEventAction()
                {
                    animator = wrapper.animator,
                    events = [FsmEvent.GetFsmEvent("FINISHED"), FsmEvent.GetFsmEvent("EVADE")],
                    weights = [1f, 0f],
                    shortenEventTIme = 0.2f
                }
            ]
        );
        BindFsmState.Actions = list.ToArray();
        BindFsmState.Transitions = BindFsmState.Transitions.Append(new FsmTransition()
        {
            FsmEvent = FsmEvent.GetFsmEvent("EVADE"),
            ToState = "Evade To Wind Blade",
            ToFsmState = fsm.Fsm.GetState("Evade To Wind Blade")
        }).ToArray();
        BindFsmState.Transitions = BindFsmState.Transitions.Append( new FsmTransition()
        {
            FsmEvent = FsmEvent.GetFsmEvent("CHALLENGE"),
            ToState = "Teleport 1 Pre",
            ToFsmState = fsm.Fsm.GetState("Teleport 1 Pre")
        }).ToArray();
    }

    public override void SetupPhase1Modifiers()
    {
    }

    public override void SetupPhase2Modifiers()
    {
    }

    public override void SetupPhase3Modifiers()
    {
        var watchAnim = BindFsmState.Actions.FirstOrDefault(action => action is AnimEndSendRandomEventAction) as AnimEndSendRandomEventAction;
        watchAnim.events = [FsmEvent.GetFsmEvent("FINISHED"), FsmEvent.GetFsmEvent("EVADE"), FsmEvent.GetFsmEvent("CHALLENGE")];
        watchAnim.weights = [0.6f, 0.35f, 0.05f]; 
    }
}