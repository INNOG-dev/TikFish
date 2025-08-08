using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDebugger : MonoBehaviour
{

    public Entity entity;

    public string targetEntityUsername;

    public GameObject targetEntity;

    public string username;

    public void setDebuggingEntity(Entity entity)
    {
        this.entity = entity;
    }

    // Update is called once per frame
    void Update()
    {
        if(entity is LivingEntity)
        {
            LivingEntity living = (LivingEntity) entity;

            if(living.getTargetEntity() is LivingEntity)
            {
                LivingEntity targetLiving = (LivingEntity)living.getTargetEntity();

                targetEntityUsername = targetLiving.getTag().text;
            }

            if(living.getTargetEntity() != null)
            {
                targetEntity = living.getTargetEntity().GetGameObject();
            }
            else
            {
                targetEntity = null;
            }

            username = living.getTag().text;
        }
    }
}
