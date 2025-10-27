using System.Collections.Specialized;
using HutongGames.PlayMaker;
using UnityEngine;

namespace KarmelitaPrime;

public class TeleportAction : FsmStateAction
{
    public Transform Target;
    public Transform Base;
    public bool IsTeleportToBack;
    public Vector2 OverrideDirection;
    public bool AllowY;
    public float MinX;
    public float MaxX;  
    public int MaxAttempts;
    public float MinTeleportDistance; 
    public float MaxTeleportDistance;
    public override void OnEnter()
    {
        base.OnEnter();
        Vector2 direction = Vector2.zero;
        if (IsTeleportToBack)
        {
            if (Target.transform.localScale.x > 0f) // Facing Left
                direction = Vector2.right; 
            else if (Target.transform.localScale.x < 0f) // Facing Right
                direction = Vector2.left; 
        }
        else if (OverrideDirection != Vector2.zero)
        {
            direction = OverrideDirection;
        }
        else
        {
            if (Target.transform.localScale.x > 0f) 
                direction = Vector2.left; 
            else if (Target.transform.localScale.x < 0f) 
                direction = Vector2.right; 
        }

        for (int i = 0; i < MaxAttempts; i++)
        {
            float offset = Random.Range(MinTeleportDistance, MaxTeleportDistance);
            Vector2 candidatePosition = (Vector2)Target.position + (offset * direction);

            if (i == MaxAttempts - 1)
                candidatePosition.x = Mathf.Clamp(candidatePosition.x, MinX, MaxX);
            else if (candidatePosition.x < MinX || candidatePosition.x > MaxX)
                continue;
            
            if (AllowY && Target.transform.position.y <= Base.transform.position.y)
            {
                candidatePosition.y = Target.transform.position.y;
            }
            Vector3 finalPosition = new Vector3(candidatePosition.x, 
                AllowY ? candidatePosition.y : Base.position.y, 
                Base.position.z);
            Base.position = finalPosition;
            return; 
        }
    }
}