using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class Cyclone4Modifier(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Cyclone 4";
    public override void OnCreateModifier()
    {
        
    }

    public override void SetupPhase1Modifiers()
    {
        var animEvent = BindFsmState.Actions.FirstOrDefault(action => action is Tk2dWatchAnimationEvents);
        var newActions = BindFsmState.Actions.ToList();
        newActions.Remove(animEvent);
        newActions.Add(new AnimEndSendRandomEventAction()
        {
            animator = wrapper.animator,
            events = [FsmEvent.GetFsmEvent("FINISHED"), FsmEvent.GetFsmEvent("ATTACK")],
            weights = [0.5f, 0.5f],
            shortenEventTIme = 0.7f
        });
        BindFsmState.Actions = newActions.ToArray();
        var newTransitions = BindFsmState.Transitions.ToList();
        newTransitions.AddRange([
            new FsmTransition()
            {
                FsmEvent = FsmEvent.GetFsmEvent("DASH GRIND"),
                ToState = "Set Dash Grind",
                ToFsmState = fsm.Fsm.GetState("Set Dash Grind")
            },
            new FsmTransition()
            {//CHANGE THIS FUCKING SHIT TRANSITION
                FsmEvent = FsmEvent.GetFsmEvent("ATTACK"),
                ToState = "Slash Antic",
                ToFsmState = fsm.Fsm.GetState("Slash Antic")
            },
            new FsmTransition()
            {
                FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                ToState = "Face Away",
                ToFsmState = fsm.Fsm.GetState("Face Away")
            }
        ]);
        BindFsmState.Transitions = newTransitions.ToArray();
    }

    public override void SetupPhase2Modifiers()
    {
        for (int i = 0; i < BindFsmState.Actions.Length; i++)
        {
            if (BindFsmState.Actions[i] is AnimEndSendRandomEventAction animEnd)
            {
                animEnd = new AnimEndSendRandomEventAction()
                {
                    animator = wrapper.animator,
                    events = [FsmEvent.GetFsmEvent("FINISHED"), FsmEvent.GetFsmEvent("THROW")],
                    weights = [.8f, .2f], 
                    shortenEventTIme = 0.4f
                };
                BindFsmState.Actions[i] = animEnd;
            }
        }
    }

    public override void SetupPhase3Modifiers()
    {
        for (int i = 0; i < BindFsmState.Actions.Length; i++)
        {
            if (BindFsmState.Actions[i] is AnimEndSendRandomEventAction animEnd)
            {
                animEnd = new AnimEndSendRandomEventAction()
                {
                    animator = wrapper.animator,
                    events = [FsmEvent.GetFsmEvent("DASH GRIND"), FsmEvent.GetFsmEvent("FINISHED"), FsmEvent.GetFsmEvent("THROW")],
                    weights = [.3f, .3f, .4f],
                    shortenEventTIme = 0.4f
                };
                BindFsmState.Actions[i] = animEnd;
            }
        }
    }
}