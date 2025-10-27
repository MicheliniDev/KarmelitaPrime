using System.Collections.Specialized;
using HutongGames.PlayMaker;
using UnityEngine;

namespace KarmelitaPrime;

public class TeleportAction : FsmStateAction
{
    public Transform Target;
    public Transform Base;
    public bool IsTeleportToBack;
    public float OffsetY;
    public Vector2 OverrideDirection;
    public bool TeleportToFacing;
    public bool AllowY;
    public bool KeepY;
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
        else if (TeleportToFacing)
        {
            if (Base.transform.localScale.x > 0f) 
                direction = Vector2.left; 
            else if (Base.transform.localScale.x < 0f) 
                direction = Vector2.right; 
        }
        else if (OverrideDirection != Vector2.zero)
        {
            direction = OverrideDirection.normalized; 
        }
        else
        {
            if (Target.transform.localScale.x > 0f) 
                direction = Vector2.left; 
            else if (Target.transform.localScale.x < 0f) 
                direction = Vector2.right; 
        }

        float minDistance = Mathf.Abs(MinTeleportDistance);
        float maxDistance = Mathf.Abs(MaxTeleportDistance);
        
        float offset = Random.Range(minDistance, maxDistance);

        for (int i = 0; i < MaxAttempts; i++)
        {
            Vector2 candidatePosition = (Vector2)Target.position + (offset * direction);

            if (i == MaxAttempts - 1)
                candidatePosition.x = Mathf.Clamp(candidatePosition.x, MinX, MaxX);
            else if (candidatePosition.x < MinX || candidatePosition.x > MaxX)
                continue;

            float finalY;
            
            if (!AllowY)
                finalY = Base.position.y;
            else if (KeepY)
                finalY = Base.position.y;
            else
                finalY = Target.position.y;
            finalY += OffsetY;
            finalY = Mathf.Clamp(finalY, 21.4353f, 34.9679f);
            Vector3 finalPosition = new Vector3(
                candidatePosition.x, 
                finalY, 
                Base.position.z 
            );
            
            Base.position = finalPosition;
            return; 
        }
    }
}