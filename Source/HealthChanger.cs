using System.Reflection;

namespace KarmelitaPrime;

public class HealthChanger
{
    public static HealthChanger Initialize(HealthManager target, int maxHP)
    {
        HealthChanger instance = new HealthChanger();
        
        FieldInfo maxHPField = typeof(HealthManager).GetField("initHp", BindingFlags.NonPublic | BindingFlags.Instance);
        int value = (int)maxHPField.GetValue(target);
        maxHPField.SetValue(target, maxHP);
        
        target.HealToMax();
        return instance;
    }
}