using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class AttackChoiceModifier(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Attack Choice";
    public override void OnCreateModifier()
    {
        /*BindFsmState.Transitions = BindFsmState.Transitions.Append(
            new FsmTransition()
            {
                FsmEvent = FsmEvent.GetFsmEvent("APPROACH"),
                ToState = "Long Approach",
                ToFsmState = fsm.Fsm.GetState("Long Approach")
            }).ToArray();
        BindFsmState.Actions = BindFsmState.Actions.Prepend(new CheckHeroTooCloseAction()
        {
            Owner = wrapper.gameObject,
            Threshold = 2f,
            TrueEvent = FsmEvent.GetFsmEvent("APPROACH")
        }).ToArray();*/
    }

    public override void SetupPhase1Modifiers()
    {
    }

    public override void SetupPhase2Modifiers()
    {
        var random2Action = BindFsmState.Actions.FirstOrDefault(
                action => action is SendRandomEventV4 eventV4 
                          && eventV4.events.Length == 6) 
            as SendRandomEventV4;
        var eventList = random2Action.events.ToList();
        var weightList = random2Action.weights.ToList();
        eventList.Remove(eventList.FirstOrDefault(weight => weight.Name == "DASH GRIND"));
        eventList.Remove(eventList.FirstOrDefault(weight => weight.Name == "JUMP SPIN"));
        weightList.Remove(weightList.FirstOrDefault(weight => weight.Value == 0.5f));
        weightList.Remove(weightList.FirstOrDefault(weight => weight.Value == 1F));
        random2Action.events = eventList.ToArray();
        random2Action.weights = weightList.ToArray();
    }

    public override void SetupPhase3Modifiers()
    {
    }
}