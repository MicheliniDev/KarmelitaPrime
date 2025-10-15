using HutongGames.PlayMaker;

namespace KarmelitaPrime;

public class LogAction : FsmStateAction
{
    public string Message;
    public override void OnEnter()
    {
        base.OnEnter();
        KarmelitaPrimeMain.Instance.Log(Message);
    }
}