using System.Linq;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace KarmelitaPrime;

public class Rethrow3State(
    PlayMakerFSM fsm,
    PlayMakerFSM stunFsm,
    KarmelitaWrapper wrapper,
    KarmelitaFsmController fsmController)
    : StateModifierBase(fsm, stunFsm, wrapper, fsmController)
{
    public override string BindState => "Rethrow 3";
    public override void OnCreateModifier()
    {
        CreateBindState();
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
    
    private void CreateBindState()
    {
        var bindState = new FsmState(fsm.Fsm)
        {
            Name = BindState,
            Actions = 
            [
                new AnimationPlayerAction()
                {
                    animator = wrapper.animator,
                    ClipName = "Throw",
                    AnimationFinishedEvent = FsmEvent.GetFsmEvent("FINISHED"),
                    shortenEventTIme = 0.4f
                },
                new PlayRandomClipAction()
                {
                    Table = wrapper.AttackQuickTable,
                    Source = fsm.Fsm.GetFsmGameObject("Audio Loop Voice").Value
                },
                new EnableGameObjectAction()
                {
                    GameObject = fsm.Fsm.GetFsmGameObject("Throw Slash").Value,
                    Enable = true,
                    ResetOnExit = true,
                },
                new CheckHeroYAction()
                {
                    Target = wrapper.transform,
                    AboveEvent = FsmEvent.GetFsmEvent("CANCEL")
                }
            ],
            Transitions = [
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("FINISHED"),
                    ToState = "Rethrow 3 Throw",
                    ToFsmState = fsm.Fsm.GetState("Rethrow 3 Throw")
                },
                new FsmTransition()
                {
                    FsmEvent = FsmEvent.GetFsmEvent("CANCEL"),
                    ToState = "Jump Antic",
                    ToFsmState = fsm.Fsm.GetState("Jump Antic")
                }
            ]
        };
        fsm.Fsm.States = fsm.Fsm.States.Append(bindState).ToArray();
    }
}