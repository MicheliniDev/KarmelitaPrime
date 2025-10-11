using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine.UIElements.Experimental;

namespace KarmelitaPrime;

public class Slash3Modifier(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Slash 3";
    private FsmState transitioner;
    private FsmEvent toSlash3OnlyState => FsmEvent.GetFsmEvent("SLASH COMBO");
    private FsmEvent toDashGrindEvent => FsmEvent.GetFsmEvent("DASH GRIND");
    public override void OnCreateModifier()
    {
        CreateTransitionerState();
    }

    public override void SetupPhase1Modifiers()
    {
        //SetTransitionToTransitionerState();
        SetBindStateToTransitionerStateTransition();
        SetTransitionerStateTransitions();
    }

    public override void SetupPhase2Modifiers()
    {
    }

    public override void SetupPhase3Modifiers()
    {
    }

    private void CreateTransitionerState()
    {
        transitioner = new FsmState(fsm.Fsm)
        {
            Name = "Slash 3 Transitioner",
            Actions =
            [
                new WeightedRandomEventAction()
                {
                    events = [toDashGrindEvent, toSlash3OnlyState],
                    weights = [0.4f, 0.6f],
                }
            ]
        };
        fsm.Fsm.States = fsm.FsmStates.Append(transitioner).ToArray();
    }
    
    
     private void SetBindStateToTransitionerStateTransition()
     {
        //CLEARING OLD TRANSITIONS
        var transitions = BindFsmState.Transitions.ToList();
        transitions.Clear();

        //ADD NEW STATE TO OLD STATE TRANSITIONS
        transitions.Add(new FsmTransition()
        {
            FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
            ToState = transitioner.Name,
            ToFsmState = transitioner
        });
        BindFsmState.Transitions = transitions.ToArray();
    }

     private void SetTransitionerStateTransitions()
     {
         FsmEvent toSlash3OnlyState = FsmEvent.GetFsmEvent("SLASH COMBO");
         FsmEvent toDashGrindEvent = FsmEvent.GetFsmEvent("DASH GRIND");
         transitioner.Transitions =
         [
             new FsmTransition()
             {
                 FsmEvent = toDashGrindEvent,
                 ToState = "Set Dash Grind",
                 ToFsmState = fsm.Fsm.GetState("Set Dash Grind"),
             },
             new FsmTransition()
             {
                 FsmEvent = toSlash3OnlyState,
                 ToState = "Slash3OnlyState",
                 ToFsmState = fsm.Fsm.GetState("Slash3OnlyState"),
             }
         ];
     }
}