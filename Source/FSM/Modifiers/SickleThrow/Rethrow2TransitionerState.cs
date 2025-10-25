using System.Linq;
using HutongGames.PlayMaker;

namespace KarmelitaPrime;

public class Rethrow2TransitionerState(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Rethrow 2 Transitioner";
    public override void OnCreateModifier()
    {
        CreateBindState();
    }

    public override void SetupPhase1Modifiers()
    {
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
                    events = [FsmEvent.GetFsmEvent("FINISHED"), FsmEvent.GetFsmEvent("ATTACK")],
                    weights = [0.5f, 0.5f],
                };
                BindFsmState.Actions[i] = animEnd;
            }
        }
        
        BindFsmState.Transitions = BindFsmState.Transitions.Append(new FsmTransition()
        {
            FsmEvent = FsmEvent.GetFsmEvent("ATTACK"),
            ToState = "Counter Attack",
            ToFsmState = fsm.Fsm.GetState("Counter Attack")
        }).ToArray();
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
                    events = [FsmEvent.GetFsmEvent("ATTACK"), FsmEvent.GetFsmEvent("THROW")],
                    weights = [0.5f, 0.5f],
                };
                BindFsmState.Actions[i] = animEnd;
            }
        }
        
        BindFsmState.Transitions = BindFsmState.Transitions.Append(new FsmTransition()
        {
            FsmEvent = FsmEvent.GetFsmEvent("THROW"),
            ToState = "Rethrow 3",
            ToFsmState = fsm.Fsm.GetState("Rethrow 3")
        }).ToArray();
    }
    
    private void CreateBindState()
    {
        var bindState = new FsmState(fsm.Fsm)
        {
            Name = BindState,
            Actions = [new AnimEndSendRandomEventAction()
            {
                animator = wrapper.animator,
                events = [FsmEvent.GetFsmEvent("FINISHED")],
                weights = [1f],
            }],
        };
        fsm.Fsm.States = fsm.Fsm.States.Append(bindState).ToArray();

        BindFsmState.Transitions = [
            new FsmTransition()
            {
                FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                ToState = "Start Idle",
                ToFsmState = fsm.Fsm.GetState("Start Idle")
            },
        ];
    }
}