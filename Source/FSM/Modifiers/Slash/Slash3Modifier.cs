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
    public override void OnCreateModifier()
    {
    }

    public override void SetupPhase1Modifiers()
    {
        SetToTransitioner();
    }

    public override void SetupPhase2Modifiers()
    {
    }

    public override void SetupPhase3Modifiers()
    {
    }
    
    private void SetToTransitioner()
    {
        var transitions = BindFsmState.Transitions.ToList();
        transitions.Clear();

        transitions.Add(new FsmTransition()
        {
            FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
            ToState = "Slash 3 Transitioner",
            ToFsmState = fsm.Fsm.GetState("Slash 3 Transitioner")
        });
        BindFsmState.Transitions = transitions.ToArray(); 
    }
}