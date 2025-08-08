using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect
{

    protected float activeTime;

    private EffectProperty property;

    public Effect(EffectProperty property)
    {
        this.property = property;
    }

    public void resetEffect()
    {
        activeTime = 0;
    }

    public virtual void OnEffectStart(LivingEntity entity)
    {

    }

    public virtual void OnEffectFinished(LivingEntity entity)
    {

    }

    public virtual void ApplyEffect(LivingEntity entity)
    {
        activeTime += Time.deltaTime;
    }

    public float GetActiveTime()
    {
        return activeTime;
    }

    public EffectProperty GetEffectProperty()
    {
        return property;
    }

}
