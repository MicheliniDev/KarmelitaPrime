using System.Linq;
using HutongGames.PlayMaker;

namespace KarmelitaPrime;

public class SickleRethrow2Modifier : StateModifierBase
{
    public SickleRethrow2Modifier(PlayMakerFSM fsm, PlayMakerFSM stunFsm, KarmelitaWrapper wrapper, KarmelitaFsmController fsmController) : base(fsm, stunFsm, wrapper, fsmController)
    {
    }

    public override string BindState => "Rethrow 2";
    private FsmEvent finishedEvent => FsmEvent.GetFsmEvent("FINISHED");
    public override void OnCreateModifier()
    {
    }

    public override void SetupPhase1Modifiers()
    {
        var transitions = BindFsmState.Transitions.ToList();
        transitions.Clear();
        transitions.Add(new FsmTransition()
        {
            FsmEvent = finishedEvent,
            ToFsmState = fsm.Fsm.GetState("Cyclone Antic"),
            ToState = "Cycle Antic"
        });
    }

    public override void SetupPhase2Modifiers()
    {
        throw new System.NotImplementedException();
    }

    public override void SetupPhase3Modifiers()
    {
        throw new System.NotImplementedException();
    }
}