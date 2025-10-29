using System.Linq;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class Slash1Modifier(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Slash 1";
    public override void OnCreateModifier()
    {
    }

    public override void SetupPhase1Modifiers()
    {
        BindFsmState.Actions = BindFsmState.Actions.Prepend(new FaceHeroAction()
        {
            Transform = wrapper.transform
        }).ToArray();
        var velocityAction = BindFsmState.Actions.FirstOrDefault(action => action is SetVelocityByScale) as SetVelocityByScale;
        velocityAction!.speed.Value *= 1.3f;
    }

    public override void SetupPhase2Modifiers()
    {
    }

    public override void SetupPhase3Modifiers()
    {
    }
}