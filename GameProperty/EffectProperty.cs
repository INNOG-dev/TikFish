using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectProperty
{
    private int effectId;

    private float duration;

    private int amplifier;

    private Type effectType;

    public EffectProperty setEffectType(Type effectType)
    {
        this.effectType = effectType;
        return this;
    }

    public EffectProperty setEffectId(int id)
    {
        this.effectId = id;
        return this;
    }

    public EffectProperty setEffectDuration(float duration)
    {
        this.duration = duration;
        return this;
    }

    public EffectProperty setEffectAmplifier(int amplifier)
    {
        this.amplifier = amplifier;
        return this;
    }

    public Type getEffectType()
    {
        return effectType;
    }

    public float GetDuration()
    {
        return duration;
    }

    public int getId()
    {
        return effectId;
    }

}
