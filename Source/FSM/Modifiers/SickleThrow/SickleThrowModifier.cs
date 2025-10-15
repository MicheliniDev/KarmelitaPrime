namespace KarmelitaPrime;

public class SickleThrowModifier : StateModifierBase
{
    public SickleThrowModifier(PlayMakerFSM fsm, PlayMakerFSM stunFsm, KarmelitaWrapper wrapper, KarmelitaFsmController fsmController) : base(fsm, stunFsm, wrapper, fsmController)
    {
    }

    public override string BindState { get; }
    public override void OnCreateModifier()
    {
        throw new System.NotImplementedException();
    }

    public override void SetupPhase1Modifiers()
    {
        throw new System.NotImplementedException();
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