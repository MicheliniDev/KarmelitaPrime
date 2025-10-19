using HutongGames.PlayMaker.Actions;

namespace KarmelitaPrime;

public class JumpLaunchModifier(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Jump Launch";
    public override void OnCreateModifier()
    {
    }

    public override void SetupPhase1Modifiers()
    {
        ChangeActionValues(0.30f, 80f);
    }

    public override void SetupPhase2Modifiers()
    {
        ChangeActionValues(0.27f, 80f);
    }

    public override void SetupPhase3Modifiers()
    {
        ChangeActionValues(0.22f, 80f);
    }
    
    private void ChangeActionValues(float waitTime = 0.35f, float yspeed = 65f)
    {
        foreach (var action in BindFsmState.Actions) {
            if (action is Wait wait) {
                wait.time = waitTime;
            }

            if (action is SetVelocityByScale velocity)
            {
                velocity.ySpeed = yspeed;
            }
        }
    }
}