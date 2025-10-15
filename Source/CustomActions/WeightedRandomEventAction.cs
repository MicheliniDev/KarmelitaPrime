using HutongGames.PlayMaker;

namespace KarmelitaPrime;

public class WeightedRandomEventAction : FsmStateAction
{
    public FsmEvent[] events;
    public float[] weights;
    
    public override void OnEnter()
    {
        float total = 0;
        
        for (int i = 0; i < weights.Length; i++) 
            total += weights[i];
        
        float roll = UnityEngine.Random.value * total;
        float cumulative = 0f;
        for (int i = 0; i < events.Length; i++)
        {
            cumulative += weights[i];
            if (roll <= cumulative)
            {
                Fsm.Event(events[i]);
                break;
            }
        }
    }
}