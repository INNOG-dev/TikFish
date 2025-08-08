using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Spine.Unity;
using System;

public class LivingEntity : Entity
{
    protected int direction;

    private List<Effect> activesEffects;

    protected float timer;

    protected TextMeshPro NameTag;

    private SkeletonAnimation entitySkeleton;

    private List<Material> entityMaterials;

    private MeshRenderer modelRenderer;

    protected FieldOfView fieldOfView;

    protected Entity targetEntity;

    protected float directionChangeCooldown;


    public enum HorizontalDirection
    {
        LEFT = -1,
        RIGHT = 1
    };

    public enum VerticalDirection
    {
        UP = 1,
        DOWN = -1
    };






    public LivingEntity()
    {
        activesEffects = new List<Effect>();

        entityMaterials = new List<Material>();
    }



    public override void Init(GameObject model, string id)
    {
        NameTag = model.GetComponentInChildren<TextMeshPro>();

        entitySkeleton = model.GetComponentInChildren<SkeletonAnimation>();

        modelRenderer = model.GetComponentInChildren<MeshRenderer>();
        
        entityMaterials.AddRange(modelRenderer.materials);

        entitySkeleton.OnMeshAndMaterialsUpdated += UpdateMaterial;

        base.Init(model, id);
    }

    protected override void InitColliders()
    {
        base.InitColliders();
    }

    private void UpdateMaterial(SkeletonRenderer renderer)
    {
        modelRenderer.materials = entityMaterials.ToArray();
    }

    public void setHorizontalDirection(HorizontalDirection dir)
    {
        if(directionChangeCooldown <= 0)
        {
            directionChangeCooldown = 0.30F;
            direction = (int)dir;
        }
    }

    public HorizontalDirection getHorizontalDirection()
    {
        return (HorizontalDirection) direction;
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        UpdateOrientation();

        List<Effect> finishedEffects = new List<Effect>();

        foreach (Effect effect in activesEffects)
        {
            if (effect != null)
            {
                if(timer % 2 == 0) Debug.Log("a Effect is active remaining time : " + (effect.GetEffectProperty().GetDuration() - effect.GetActiveTime()));
                effect.ApplyEffect(this);

                if(effect.GetActiveTime() >= effect.GetEffectProperty().GetDuration())
                {
                    effect.OnEffectFinished(this);
                    finishedEffects.Add(effect);
                }
            }
        }

        activesEffects.RemoveAll(x => finishedEffects.Contains(x));
    }

    protected virtual void UpdateOrientation()
    {
        if (direction < 0)
        {
            gameObject.transform.GetChild(0).localScale = new Vector2(1, 1);
        }
        else if (direction > 0)
        {
            gameObject.transform.GetChild(0).localScale = new Vector2(-1, 1);
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        fieldOfView.OnDrawGizmos();
    }

    public virtual float GetSize()
    {
        return 1f;
    }

    protected virtual void HandleMovement()
    {

    }

    public override void onDestroy()
    {
       
    }

    public void AddEffect(Effect effect, float duration)
    {
        EffectProperty property = new EffectProperty();
        
        property.setEffectAmplifier(1);
        property.setEffectId(effect.GetEffectProperty().getId());
        property.setEffectDuration(duration);
        property.setEffectType(effect.GetEffectProperty().getEffectType());

        AddEffect((Effect)Activator.CreateInstance(property.getEffectType(), property));
    }

    public void AddEffect(Effect effect)
    {

        bool isActive = false;
        foreach(Effect activeEffect in activesEffects)
        {
            if(activeEffect.GetEffectProperty().getId() == effect.GetEffectProperty().getId())
            {
                activeEffect.resetEffect();
                isActive = true;
                break;
            }
        }

        if(!isActive)
        {
            effect.OnEffectStart(this);
            activesEffects.Add(effect);
        }
    }

    public bool isEffectActive(int effectId)
    {
        return activesEffects.Find(x => x.GetEffectProperty().getId() == effectId) != null;
    }

    public bool isEffectActive(EffectProperty property)
    {
        return isEffectActive(property.getId());
    }

    public virtual bool isInvincible()
    {
        return GameLogic.instance.getPartyManager().getGameState() != GameState.STARTED;
    }

    public void setTagName(string username)
    {
        NameTag.text = username;
    }

    public TextMeshPro getTag()
    {
        return NameTag;
    }

    public SkeletonAnimation getEntitySkeleton()
    {
        return entitySkeleton;
    }

    public void addMaterial(Material material)
    {
        entityMaterials.Add(material);
    }

    public void removeMaterial(Material material)
    {
        entityMaterials.Remove(material);
    }

    public void removeMaterial(int materialIndex)
    {
        entityMaterials.RemoveAt(materialIndex);
    }

    public MeshRenderer getModelRenderer()
    {
        return modelRenderer;
    }

    public FieldOfView getFieldOfView()
    {
        return fieldOfView;
    }

    public Entity getTargetEntity()
    {
        return targetEntity;
    }




}
