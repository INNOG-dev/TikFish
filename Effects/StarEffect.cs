using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarEffect :  Effect
{
    private Material effectMaterial;


    public StarEffect(EffectProperty property) : base(property)
    {
        effectMaterial = Resources.Load<Material>("Shaders/Effects/Star/Star");
    }

    public override void OnEffectStart(LivingEntity entity)
    {
        Material material = new Material(effectMaterial);

        material.SetTexture("_MainTex", entity.getModelRenderer().materials[0].mainTexture);

        entity.addMaterial(material);
        entity.setAdditionnalVelocity(new Vector2(2, 1));
        

        base.OnEffectStart(entity);
    }

    public override void OnEffectFinished(LivingEntity entity)
    {
        entity.removeMaterial(1);

        entity.setAdditionnalVelocity(Vector2.one);


        base.OnEffectFinished(entity);
    }

    public override void ApplyEffect(LivingEntity entity)
    {
        base.ApplyEffect(entity);
    }

}
