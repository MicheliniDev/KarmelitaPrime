using System.Reflection;
using UnityEngine;

namespace KarmelitaPrime;

public class HealthChanger
{
    public static HealthChanger Initialize(HealthManager target, int maxHP, int phase2Threshold, int phase3Threshold)
    {
        HealthChanger instance = new HealthChanger();
        
        PlayMakerFSM fsm = target.gameObject.gameObject.LocateMyFSM("Control");
        SetMaxHp(target, maxHP);
        SetPhase2Threshold(phase2Threshold, fsm);
        SetPhase3Threshold(phase3Threshold, fsm);
        return instance;
    }

    private static void SetMaxHp(HealthManager target, int maxHP)
    {
        FieldInfo maxHpField = typeof(HealthManager).GetField("initHp", BindingFlags.NonPublic | BindingFlags.Instance);
        int value = (int)maxHpField.GetValue(target);
        maxHpField.SetValue(target, maxHP);
        
        target.HealToMax();
    }

    private static void SetPhase2Threshold(int threshold, PlayMakerFSM fsm)
    {
        fsm.FsmVariables.FindFsmInt("P2 HP").Value = threshold;
    }
    
    private static void SetPhase3Threshold(int threshold, PlayMakerFSM fsm)
    {
        fsm.FsmVariables.FindFsmInt("P3 HP").Value = threshold;
    }
}