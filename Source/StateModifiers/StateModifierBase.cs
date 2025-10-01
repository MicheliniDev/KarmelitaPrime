namespace KarmelitaPrime;

public abstract class StateModifierBase
{
    public abstract string BindState { get; }
    public abstract float StateSpeed { get; }
    
    public abstract void SetPhase1Modifiers();
    public abstract void SetPhase2Modifiers();
    public abstract void SetPhase3Modifiers();
}