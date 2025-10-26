using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class Slash9Modifier(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Slash 9";
    private FsmEvent toSlash2OnlyState => FsmEvent.GetFsmEvent("SLASH COMBO");
    private FsmEvent finishedEvent => FsmEvent.GetFsmEvent("FINISHED");
    private FsmEvent toCycloneEvent => FsmEvent.GetFsmEvent("CYCLONE SPIN");
    public override void OnCreateModifier()
    {
    }
    
    public override void SetupPhase1Modifiers()
    {
        //CUSTOM ANIMATION ENDER
        var watchAnimEvent = BindFsmState.Actions.FirstOrDefault(action => action is Tk2dWatchAnimationEvents);
        var newActions = BindFsmState.Actions.ToList();
        newActions.Remove(watchAnimEvent);
        newActions.AddRange([
            new AnimEndSendRandomEventAction()
            {
                animator = wrapper.animator,
                events = [finishedEvent, toCycloneEvent],
                weights = [.5f, .5f],
                shortenEventTIme = 0.25f
            },
            new FaceHeroAction()
            {
                Transform = wrapper.transform
            }
        ]);
        BindFsmState.Actions = newActions.ToArray();
        
        BindFsmState.Transitions = [
            new FsmTransition()
            {
                FsmEvent = finishedEvent,
                ToState = "Slash End",
                ToFsmState = fsm.Fsm.GetState("Slash End"),
            },
            new FsmTransition()
            {
                FsmEvent = toCycloneEvent,
                ToState = "Cyclone Antic",
                ToFsmState = fsm.Fsm.GetState("Cyclone Antic"),
            }
        ];
    }

    public override void SetupPhase2Modifiers()
    {
        BindFsmState.Transitions = BindFsmState.Transitions.Append(new FsmTransition()
        {
            FsmEvent = toSlash2OnlyState,
            ToState = "New Slash 2 State",
            ToFsmState = fsm.Fsm.GetState("New Slash 2 State"),
        }).ToArray();
        for (int i = 0; i < BindFsmState.Actions.Length; i++)
        {
            if (BindFsmState.Actions[i] is AnimEndSendRandomEventAction)
            {
                BindFsmState.Actions[i] = new AnimEndSendRandomEventAction()
                {
                    animator = wrapper.animator,
                    events = [toSlash2OnlyState, toCycloneEvent],
                    weights = [.5f, .5f],
                    shortenEventTIme = 0.25f
                };
            }
        }
    }

    public override void SetupPhase3Modifiers()
    {
    }
}