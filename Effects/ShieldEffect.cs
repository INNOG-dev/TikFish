using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEffect : Effect
{

    private static readonly GameObject effectResource = Resources.Load<GameObject>("Shaders/Effects/Shield/ShieldEffect");

    private GameObject resourceGameObject;

    public ShieldEffect(EffectProperty property) : base(property)
    {
    }

    public override void OnEffectStart(LivingEntity entity)
    {
        base.OnEffectStart(entity);
        if (entity is Fish)
        {
            resourceGameObject = Object.Instantiate(effectResource, ((Fish)entity).getEffectsTransform());
        }
    }

    public override void OnEffectFinished(LivingEntity entity)
    {
        base.OnEffectFinished(entity);
        Object.Destroy(resourceGameObject);
    }


}
