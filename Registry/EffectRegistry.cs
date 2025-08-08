using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRegistry 
{

    private Dictionary<int, string> registriesNames = new Dictionary<int, string>();

    private Dictionary<int, EffectProperty> registries = new Dictionary<int, EffectProperty>();


    public EffectProperty register(int id, float duration, int amplifier, Type effectType, string name)
    {
        if (registriesNames.ContainsKey(id))
        {
            throw new System.Exception("Effect Id already registered");
        }

        registriesNames.Add(id, name);

        EffectProperty property = new EffectProperty().setEffectDuration(duration).setEffectAmplifier(amplifier).setEffectId(id).setEffectType(effectType);

        registries.Add(id, property);
        Debug.Log("Effect with name " + name + " id " + id + " registered");

        return property;
    }

}
