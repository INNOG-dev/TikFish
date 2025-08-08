using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FieldOfView
{

    public float viewRadius;

    [Range(0F,360F)]
    public float viewAngle;

    private CircleCollider2D fieldOfViewCollider;

    private LivingEntity entity;

    private float minTargetFindTime = 0.25F;

    private float maxTargetFindTime = 1F;

    private float currentTime;

    private List<Entity> entitiesInFOV = new List<Entity>();


    public FieldOfView(LivingEntity entity, Collider2D collider, float viewAngle, float viewRadius)
    {
        this.entity = entity;
        this.fieldOfViewCollider = (CircleCollider2D) collider;
        this.viewAngle = viewAngle;
        this.viewRadius = viewRadius;

        currentTime = Random.Range(minTargetFindTime, maxTargetFindTime);
    }

    public List<Entity> FindVisibleTargets()
    {
        GameLogic game = GameLogic.instance;
        CircleCollider2D collider = getCollider();

        float radius = viewRadius * (entity.isEffectActive(Effects.STAR_EFFECT.getId()) ? 2 : 1) * entity.GetGameObject().transform.localScale.x;

        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(collider.bounds.center, radius);

        Vector2 entityPosition = entity.getPosition();

        List<Entity> visibleTargets = new List<Entity>();

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Collider2D entityCollider = targetsInViewRadius[i];

            if(entityCollider.name == "FishCollider")
            {
                LivingEntity target = (LivingEntity)game.getEntityFromCollider(entityCollider);

                if (target == entity) continue;

                Vector2 dirToTarget = (target.getPosition() - entityPosition).normalized;

                if (Vector2.Angle(Vector2.right * (int) entity.getHorizontalDirection(), dirToTarget) < viewAngle / 2)
                {
                    float dstToTarget = Vector2.Distance(entityPosition, target.getPosition());

                    if (!Physics2D.Raycast(entityPosition, dirToTarget, dstToTarget, MapData.obstacleMask))
                    {
                        // Le poisson cible est dans le champ de vision et il n'y a pas d'obstacles entre les deux.
                        //Debug.Log(target.getUsername() + " est dans le champ de vision de " + entity.getUsername() + "!");
                        visibleTargets.Add(target);
                    }
                }
            }
            else if(entityCollider.tag == "Bonus")
            {
                Entity targetEntity = game.getEntityFromCollider(entityCollider);

                Bonus target = (Bonus)targetEntity;

                Vector2 dirToTarget = (target.getPosition() - entityPosition).normalized;

                if (Vector2.Angle(Vector2.right * (int)entity.getHorizontalDirection(), dirToTarget) < viewAngle / 2)
                {
                    float dstToTarget = Vector2.Distance(entityPosition, target.getPosition());

                    if (!Physics2D.Raycast(entityPosition, dirToTarget, dstToTarget, MapData.obstacleMask))
                    {
                        visibleTargets.Add(target);
                    }
                }
            }
        }

        return visibleTargets;

    }

    public void UpdateFOV()
    {
         if ((currentTime -= Time.deltaTime) <= 0)
         {
            currentTime = Random.Range(minTargetFindTime, maxTargetFindTime);

            entitiesInFOV = FindVisibleTargets();
         }
    }

    public bool isVisible(Entity target)
    {
        return entitiesInFOV.Contains(target);
    }

    public List<Entity> getEntitiesInFOV()
    {
        return entitiesInFOV;
    }

    public void OnDrawGizmos()
    {
        CircleCollider2D collider = (CircleCollider2D)getCollider();

        Vector2 position = getCollider().bounds.center;

        float radius = viewRadius * (entity.isEffectActive(Effects.STAR_EFFECT.getId()) ? 2 : 1) * entity.GetGameObject().transform.localScale.x;


        Vector2 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector2 viewAngleB = DirFromAngle(viewAngle / 2, false);

        Vector2 startPoint = position + viewAngleA * radius;
        Vector2 endPoint = position + viewAngleB * radius;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(position, startPoint);
        Gizmos.DrawLine(position, endPoint);

        int segments = 10; 

        for (int i = 1; i <= segments; i++)
        {
            float angle = -viewAngle / 2 + (viewAngle / segments) * i;

            Vector2 segmentDir = DirFromAngle(angle, false);

            Vector2 segmentPoint = position + segmentDir * radius;

            float previousAngle = -viewAngle / 2 + (viewAngle / segments) * (i - 1);
            Vector2 previousDir = DirFromAngle(previousAngle, false);
            Vector2 previousPoint = position + previousDir * radius;

            Gizmos.DrawLine(previousPoint, segmentPoint);
        }
    }

    public Vector2 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {

        if (!angleIsGlobal)
        {
            angleInDegrees += getCollider().transform.eulerAngles.z;
        }
        return new Vector2(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad) * (int)entity.getHorizontalDirection(), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
    }

    public CircleCollider2D getCollider()
    {
        return fieldOfViewCollider;
    }

}
