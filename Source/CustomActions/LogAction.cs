using HutongGames.PlayMaker;

namespace KarmelitaPrime;

public class LogAction : FsmStateAction
{
    public string log;
    public override void OnEnter()
    {
        base.OnEnter();
        KarmelitaPrimeMain.Instance.Log(log);
    }
}