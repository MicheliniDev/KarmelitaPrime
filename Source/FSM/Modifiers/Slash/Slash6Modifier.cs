using System.Linq;
using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class Slash6Modifier(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Slash 6";
    public override void OnCreateModifier()
    {
    }

    public override void SetupPhase1Modifiers()
    {
        BindFsmState.Actions = BindFsmState.Actions.Append(new FaceHeroAction()
        {
            Transform = wrapper.transform
        }).ToArray();
    }

    public override void SetupPhase2Modifiers()
    {
    }

    public override void SetupPhase3Modifiers()
    {
    }
}