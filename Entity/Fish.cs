using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : LivingEntity
{

    public readonly float targetDetectionRadius = 15F * ((int)GameLogic.instance.getDifficulty() + 1);

    private readonly float dangerDetectionRadius = 5F * ((int)GameLogic.instance.getDifficulty()+1);

    private readonly float maxSize = 2.0f;

    private readonly float minSize = 0.140f;

    private readonly float scoreSizeMultiplier = 0.0005995f;

    private readonly Vector2 initialVelocity = new Vector2(2, 1);

    private Controller controller;

    private float globalScore;

    private float score;

    private bool localPlayer;

    private bool isTeleported = false;

    private Transform EffectsTransform;

    protected Vector2? targetPosition;

    protected float externalForce = 1;

    protected float attackCooldown;

    private float eatOffCooldown;

    private float escapeCooldown;

    private string username;



    public Fish()
    {
        score = 0;

        velocity = new Vector2(2, 1);

        controller = new Controller();

        if (Random.Range(0, 2) == 0)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }


    }

    public void setLocalPlayer()
    {
        localPlayer = true;
    }

    public override void Init(GameObject model, string id)
    {
        base.Init(model, id);

        EffectsTransform = model.transform.GetChild(0).GetChild(1);
    }


    public override void Update()
    {

        directionChangeCooldown -= Time.deltaTime;

        if (localPlayer)
        {
            controller.UpdateInputs();

            if(controller.getDirInput().x > 0)
            {
                setHorizontalDirection(HorizontalDirection.RIGHT);
            }
            else if(controller.getDirInput().x < 0)
            {
                setHorizontalDirection(HorizontalDirection.LEFT);

            }


            if (controller.AttackPressed())
            {
                if(canAttack())
                {
                    Attack();
                }
            }

        }
        else
        {
            if(targetPosition != null)
            {
                if (Vector2.Distance(targetPosition.Value, getPosition()) < 1F)
                {
                    targetPosition = null;
                }
            }

            if (!getFieldOfView().isVisible(targetEntity))
            {
                targetEntity = null;
            }
            else
            {
                if(canAttack() && (getSqrDistance(targetEntity) <= 5F || rand.NextDouble() > 0.95F))
                {
                    Attack();
                }
            }

            IAFindTarget();

            IAEscape();


            if (timer >= Random.Range(1F, 5F) && targetEntity == null)
            {
                timer = 0.0f;
                IAMovement();
            }
               
            

        }

        if(eatOffCooldown > 0)
        {
            eatOffCooldown -= Time.deltaTime;
        }
        else
        {
            eatOffCooldown = 0.0F;
        }
              

        attackCooldown -= Time.deltaTime;

        base.Update();

        HandleMovement();

        ProcessEat();

        HandleCollision();

        UpdateSize();

    }

    public void ProcessEat()
    {
        Collider2D eatCollider = GetColliders()[1];

        var result = Physics2D.CapsuleCastAll(eatCollider.bounds.center, eatCollider.bounds.size, CapsuleDirection2D.Horizontal, 0f, Vector2.right * direction, 0.1F);

        foreach(RaycastHit2D hit in result)
        {
            if (hit.collider.gameObject.tag == "Bonus")
            {
                hit.collider.gameObject.GetComponent<BonusCollisionHandler>().onEatean(this);
                getEntitySkeleton().AnimationState.SetAnimation(0, "attack", false);
                getEntitySkeleton().AnimationState.AddAnimation(0, "run", true, 0f);

                return;
            }
            else if(hit.collider.gameObject.name == "FishCollider")
            {
                Fish fish = (Fish)GameLogic.instance.getEntity(hit.collider.transform.parent.parent.parent.gameObject);
                if(fish != null)
                {
                    if (fish == this) continue;

                    hit.collider.gameObject.transform.parent.parent.parent.GetComponent<FishCollisionHandler>().onEatean(this);
                    return;
                }
            }
        }
    }

    public bool canAttack()
    {
        return attackCooldown <= 0;
    }

    public void Attack()
    {
        attackCooldown = 6 - Mathf.Sqrt(GetSize());

        externalForce = 6F;
    }

    protected override void InitColliders()
    {
        base.InitColliders();

        CircleCollider2D collider = (CircleCollider2D) GetColliders()[2];

        collider.radius = targetDetectionRadius;

        fieldOfView = new FieldOfView(this, collider, 90, collider.radius);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = new Color(0F, 0F, 1F, 0.1F);

        Gizmos.DrawSphere(getPosition(), dangerDetectionRadius + GetSize());
    }

    public void IAFindTarget()
    {
        fieldOfView.UpdateFOV();


        if(targetEntity == null) targetEntity = getBestTarget(fieldOfView.getEntitiesInFOV());
    }


    public void IAEscape()
    {
        escapeCooldown -= Time.deltaTime;

        if (escapeCooldown > 0) return;

        escapeCooldown = Random.Range(0.25F, 0.5F);

        if (rand.NextDouble() < 0.25F) return;

        GameLogic gamelogic = GameLogic.instance;
     
        Collider2D[] hits = Physics2D.OverlapCircleAll(getPosition(), dangerDetectionRadius);

        Vector2 currentPosition = getPosition();

        float nearestDangerDistance = float.MaxValue;
        Entity nearestDanger = null;

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.name != "EatCollider")
            {
                continue;
            }

            Fish danger = (Fish)gamelogic.getEntityFromCollider(hit);

            if (!danger.isAlive()) continue;

            if (!danger.canEat(this)) continue;

            float distance = getSqrDistance(danger);

            if(distance < nearestDangerDistance)
            {
                nearestDangerDistance = distance;
                nearestDanger = danger;
            }
        }

        if (nearestDanger == null) return;

        if(targetEntity != null)
        {
            if(targetEntity.getSqrDistance(nearestDanger) < 5F)
            {
                targetEntity = null;
            }
        }

        Vector2 dangerDirection = nearestDanger.getPosition() - currentPosition;

        Vector2 newDirection = Vector2.zero;

        if(Random.Range(0,2) == 0)
        {
            newDirection.x = -dangerDirection.x;
        }
        else
        {
            newDirection.y = -dangerDirection.y;
        }

        if(newDirection.x > 0)
        {
            setHorizontalDirection(HorizontalDirection.RIGHT);
        }
        else if(newDirection.x < 0)
        {
            setHorizontalDirection(HorizontalDirection.LEFT);
        }

        targetPosition = currentPosition + newDirection;

        if (canAttack() && (getSqrDistance(nearestDanger) < 1F || rand.NextDouble() > 0.85F))
        {
            if(rand.NextDouble() < 0.25F) Attack();
        }
    }




    public void IAMovement()
    {
        if (rand.NextDouble() < 0.10f)
        {
            int rand = Random.Range(0, 2);

            if (rand == 0)
            {
                direction = -1;
            }
            else
            {
                direction = 1;
            }
        }
        

        if(rand.NextDouble() < 0.25F)
        {
            if (Random.Range(0, 2) == 0)
            {
                moveTo(VerticalDirection.UP, 5F);
            }
            else
            {
                moveTo(VerticalDirection.DOWN, 5F);
            }
        }
    }

    protected override void HandleMovement()
    {

        externalForce = Mathf.Max(1, externalForce * Mathf.Pow(0.970F, Time.deltaTime * 60));

        if (localPlayer)
        {

            gameObject.transform.Translate(velocity.x * externalForce * additionalVelocity.x * Time.deltaTime * Vector3.left * -direction);

            gameObject.transform.Translate(velocity.y * additionalVelocity.y * Time.deltaTime * Vector3.up * controller.getDirInput().y);
        }
        else
        {
            Vector2 position = getPosition();

            if (targetEntity != null) targetPosition = targetEntity.getPosition();

            if (targetPosition != null)
            {
                Vector2 move = new Vector2(targetPosition.Value.x - position.x, targetPosition.Value.y - position.y).normalized;

                if (move.y > 0.25F) move.y += (float)rand.NextDouble() * 2F;

                else if (move.y < 0.25F) move.y -= (float)rand.NextDouble() * 2F;

                gameObject.transform.Translate(velocity.x * externalForce * additionalVelocity.x * Time.deltaTime * Vector3.left * -direction);
                gameObject.transform.Translate(velocity.y * additionalVelocity.y * Time.deltaTime * Vector2.up * move.y);
            }
            else
            {
                gameObject.transform.Translate(velocity.x * externalForce * additionalVelocity.x * Time.deltaTime * Vector3.left * -direction);
            }


        }
    }


    private void HandleCollision()
    {
        HandleCollisionVertically();
    }

    private void HandleCollisionVertically()
    {
        Collider2D collider = GetColliders()[0];

        MapData map = GameLogic.instance.getMapData();

        if (collider.bounds.max.y > map.worldBorderUP.bounds.min.y)
        {
            gameObject.transform.Translate(Vector2.down * Mathf.Abs(collider.bounds.max.y - map.worldBorderUP.bounds.min.y));
        }
        else if (collider.bounds.min.y < map.worldBorderDOWN.bounds.max.y)
        {
            gameObject.transform.Translate(Vector2.up * Mathf.Abs(collider.bounds.min.y - map.worldBorderDOWN.bounds.max.y));
        }

    }

    public void UpdateSize()
    {
        float size = Mathf.Min(minSize + GetSize(),  maxSize);

        gameObject.transform.localScale = new Vector2(size, size);

        velocity = new Vector2(initialVelocity.x - Mathf.Sqrt(Mathf.Max(0, size - 0.714F)),1);

        getEntitySkeleton().timeScale = (velocity.x * Mathf.Sqrt(additionalVelocity.x)) / maxSize;
    }

    public override float GetSize()
    {
        return score * scoreSizeMultiplier;
    }


    protected override void UpdateOrientation()
    {
        base.UpdateOrientation();
    }


    public float getScore()
    {
        return score;
    }

    public float getGlobalScore()
    {
        return score;
    }

    public void setScore(float score)
    {
        this.score = score;
    }

    public void addScore(float score)
    {
        globalScore += score;
        this.score += score;
    }

    public void onEat(Fish fish)
    {
        addScore(Mathf.Max(1,fish.getScore() / 2));
    }

    public bool canEat(Fish fish)
    {
        GameMode gamemode = GameLogic.instance.getPartyManager().getCurrentGameMode();
        return !fish.isInvincible() && gamemode.canFishEatFish(this, fish) && eatOffCooldown <= 0.0F && score > fish.score && fish.isAlive();
    }

    public Controller GetController()
    {
        return controller;
    }

    public void setTeleported(bool state)
    {
        isTeleported = state;
    }

    public bool getIsTeleported()
    {
        return isTeleported;
    }

    public string getUsername()
    {
        return username;
    }

    public void setUsername(string username)
    {
        this.username = username;
    }

    public void moveTo(VerticalDirection dir, float unit)
    {
        if(dir == VerticalDirection.UP)
        {
            targetPosition = new Vector2(getPosition().x, getPosition().y + unit);
        }
        else
        {
            targetPosition = new Vector2(getPosition().x, getPosition().y - unit);
        }
    }

    public override bool isInvincible()
    {
        return isEffectActive(Effects.INVINCIBLE_EFFECT) || base.isInvincible();
    }

    public Transform getEffectsTransform()
    {
        return EffectsTransform;
    }

    public void setEatOffCooldown(float time)
    {
        eatOffCooldown = time;
    }

    public Entity getBestTarget(List<Entity> entities)
    {
        Fish fish = this;

        Fish nearestFish = null;
        float nearestFishDistance = 0.0F;

        List<Bonus> bonus = new List<Bonus>();

        for (int i = 0; i < entities.Count; i++)
        {
            Entity entity = entities[i];

            if (entity is Fish)
            {
                Fish targetFish = (Fish)entity;
                if (fish.canEat(targetFish))
                {
                    if(nearestFish == null)
                    {
                        nearestFish = targetFish;
                        nearestFishDistance = nearestFish.getSqrDistance(fish);
                    }
                    else
                    {
                        float distance = targetFish.getSqrDistance(fish);
                        if (distance < nearestFishDistance)
                        {
                            nearestFish = targetFish;
                            nearestFishDistance = distance;
                        }
                    }
                }
            }
            else
            {
                bonus.Add((Bonus)entity);
            }  
        }

        if (nearestFish != null) return nearestFish;

        if(bonus.Count > 0) return bonus[Random.Range(0, bonus.Count)];

        return null;
    }


}
