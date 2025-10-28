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
        newActions.AddRange(
        [
            new CheckHeroTooCloseAction()
            {
                Owner = wrapper.gameObject,
                Threshold = 1f,
                TrueEvent = FsmEvent.GetFsmEvent("EVADE")//ADD EVADE STATE IN CASE HERO TOO CLOSE 
            },
            new AnimEndSendRandomEventAction()
                {
                animator = wrapper.animator,
                events = [FsmEvent.GetFsmEvent("ATTACK")],
                weights = [1f],
                shortenEventTIme = 0.5f
            }
        ]);
        BindFsmState.Actions = newActions.ToArray();
        BindFsmState.Transitions = 
        [
            new FsmTransition()
            {
                FsmEvent = FsmEvent.GetFsmEvent("DASH GRIND"),
                ToState = "Set Dash Grind",
                ToFsmState = fsm.Fsm.GetState("Set Dash Grind")
            },
            new FsmTransition()
            {
                FsmEvent = FsmEvent.GetFsmEvent("ATTACK"),
                ToState = "Jump Launch",
                ToFsmState = fsm.Fsm.GetState("Jump Launch")
            }
        ];
    }

    public override void SetupPhase2Modifiers()
    {
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
                    events = [FsmEvent.GetFsmEvent("DASH GRIND"), FsmEvent.GetFsmEvent("ATTACK")],
                    weights = [.5f, .5f],
                    shortenEventTIme = 0.5f
                };
                BindFsmState.Actions[i] = animEnd;
            }
        }
    }
}