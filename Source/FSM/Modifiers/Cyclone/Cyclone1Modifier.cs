using System.Linq;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class Cyclone1Modifier(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Cyclone 1";
    public override void OnCreateModifier()
    {
        var moveAction = BindFsmState.Actions.FirstOrDefault(action => action is SetVelocityByScale) as SetVelocityByScale;
        moveAction.speed.Value *= 1.25f;
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