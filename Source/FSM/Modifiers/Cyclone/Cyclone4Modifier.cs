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
            events = [FsmEvent.GetFsmEvent("DASH GRIND"), FsmEvent.GetFsmEvent("FINISHED")],
            weights = [.5f, .5f],
            shortenEventTIme = 0.4f
        });
        BindFsmState.Actions = newActions.ToArray();
        BindFsmState.Transitions = BindFsmState.Transitions.Append(new FsmTransition()
        {
            FsmEvent = FsmEvent.GetFsmEvent("DASH GRIND"),
            ToState = "Set Dash Grind",
            ToFsmState = fsm.Fsm.GetState("Set Dash Grind")
            
        }).ToArray();
    }

    public override void SetupPhase2Modifiers()
    {
    }

    public override void SetupPhase3Modifiers()
    {
    }
}