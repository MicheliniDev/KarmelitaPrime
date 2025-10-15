using System.Linq;
using HarmonyLib;
using HutongGames.PlayMaker;
using UnityEngine.UIElements.UIR;

namespace KarmelitaPrime;

public class CycloneAnticModifier(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Cyclone Antic";
    public override void OnCreateModifier()
    {
    }

    public override void SetupPhase1Modifiers()
    {
        var checkHeroCloneAction = new EnemyHitAction()
        {
            Owner = wrapper.health,
            OnHitEvent = FsmEvent.GetFsmEvent("BLOCKED HIT"),
            isInvincibleOnEnter = true
        };
        var toBlockTransition = new FsmTransition()
        {
            FsmEvent = FsmEvent.GetFsmEvent("BLOCKED HIT"),
            ToState = "Approach Block",
            ToFsmState = fsm.Fsm.GetState("Approach Block"),
        };
        BindFsmState.Actions = BindFsmState.Actions.Append(checkHeroCloneAction).ToArray();
        BindFsmState.Transitions = BindFsmState.Transitions.Append(toBlockTransition).ToArray();
    }

    public override void SetupPhase2Modifiers()
    {
    }

    public override void SetupPhase3Modifiers()
    {
    }
}