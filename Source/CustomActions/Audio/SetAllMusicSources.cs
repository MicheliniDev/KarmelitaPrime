using HutongGames.PlayMaker;

namespace KarmelitaPrime;

public class SetAllMusicSources : FsmStateAction
{
    public bool Active;
    public bool IsOnExit;
    public override void OnEnter()
    {
        base.OnEnter();
        if (!IsOnExit)
            DoMusicSources();
    }

    public override void OnExit()
    {
        base.OnExit();
        if (IsOnExit)
            DoMusicSources();
    }

    private void DoMusicSources()
    {
        foreach (var source in AudioManager.Instance.MusicSources)
        {
            if (Active)
                source.UnPause();
            else
                source.Pause();
        }
    }
}