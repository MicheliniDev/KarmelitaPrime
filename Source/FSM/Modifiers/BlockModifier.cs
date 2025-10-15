namespace KarmelitaPrime;

public class BlockModifier : StateModifierBase
{
    public BlockModifier(PlayMakerFSM fsm, PlayMakerFSM stunFsm, KarmelitaWrapper wrapper, KarmelitaFsmController fsmController) : base(fsm, stunFsm, wrapper, fsmController)
    {
    }

    public override string BindState { get; }
    public override void OnCreateModifier()
    {
        
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