using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class Slash3Modifier(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    protected override string BindState => "Slash 3";
    public override void SetupPhase1Modifiers()
    {
        TestTransition();
    }
    
    public override void SetupPhase2Modifiers()
    {
    }

    public override void SetupPhase3Modifiers()
    {
    }
    
    private void TestTransition()
    {
        //TRANSITION TO PHASE 2 TEST TRANSITION DO NOT SHIP ON PRODUCTION
        //PLEASE DON'T DO IT OH GOD NO PLEASE NO NO NOOOOOOOOOOOOOOOOO
        List<FsmTransition> BindFsmStateTransitions = BindFsmState.Transitions.ToList();
        BindFsmStateTransitions.Clear();
        FsmTransition transition = new FsmTransition()
        {
            FsmEvent = fsm.FsmEvents.FirstOrDefault(fsmEvent => fsmEvent.Name == "FINISHED"),
            ToState = "Set Dash Grind",
            ToFsmState = fsm.Fsm.GetState("Set Dash Grind"),
        };
        BindFsmStateTransitions.Add(transition);
        BindFsmState.Transitions = BindFsmStateTransitions.ToArray();
    }
}