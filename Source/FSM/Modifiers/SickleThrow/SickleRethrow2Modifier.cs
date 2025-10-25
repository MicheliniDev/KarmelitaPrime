using System.Linq;
using HutongGames.PlayMaker;

namespace KarmelitaPrime;

public class SickleRethrow2Modifier(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Rethrow 2";
    public override void OnCreateModifier()
    {
        BindFsmState.Transitions = [
            new FsmTransition()
            {
                FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                ToState = "Rethrow 2 Transitioner",
                ToFsmState = fsm.Fsm.GetState("Rethrow 2 Transitioner")
            }
        ];
    }

    public override void SetupPhase1Modifiers()
    {
    }

    public override void SetupPhase2Modifiers()
    {
        
    }

    public override void SetupPhase3Modifiers()
    {
    }
}