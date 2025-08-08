using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : Entity
{

    private Effect effect;

    public Bonus(EffectProperty effectProperty)
    {
        if(effectProperty.getEffectType() != null)
        {
            effect = (Effect)Activator.CreateInstance(effectProperty.getEffectType(),new object[] { effectProperty });
        }
        else
        {
            effect = new Effect(effectProperty);
        }
    }

    public override void Update()
    {

    }

    protected override void InitColliders()
    {
        colliders = new Collider2D[1];
        colliders[0] = gameObject.GetComponent<Collider2D>();
    }

    public Effect GetEffect()
    {
        return effect;
    }

    public override void onDestroy()
    {
        GameLogic.instance.bonus.Remove(this);
    }
}
