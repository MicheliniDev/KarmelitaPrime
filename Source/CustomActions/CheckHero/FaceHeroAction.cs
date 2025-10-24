using HutongGames.PlayMaker;
using UnityEngine;

namespace KarmelitaPrime;

public class FaceHeroAction : FsmStateAction
{
    public Transform Transform;
    public override void OnEnter()
    {
        base.OnEnter();
        if (HeroController.instance.transform.position.x > Transform.position.x && Transform.localScale.x > 0 ||
            HeroController.instance.transform.position.x < Transform.position.x && Transform.localScale.x < 0) {
            Vector3 scale = Transform.localScale;
            scale.x *= -1;
            Transform.localScale = scale;
        }
        Finish();
    }
}