using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishCollisionHandler : CollisionHandler
{

    public override void onEatean(Fish by)
    {
        Entity entity = GameLogic.instance.getEntity(gameObject);

        if (!(entity is Fish))
        {
            return;
        }


        Fish targetFish = (Fish)entity;

        Fish theFish = by;

        if (theFish.canEat(targetFish))
        {

            theFish.onEat(targetFish);

            theFish.getEntitySkeleton().AnimationState.SetAnimation(0, "attack", false);
            theFish.getEntitySkeleton().AnimationState.AddAnimation(0, "run", true, 0f);

            targetFish.setDead(true);
            targetFish.GetGameObject().SetActive(false);
        }
    }
        


    /*public override void OnTriggerEnter2D(Collider2D collision)
    { 
        
        if (collision.gameObject.name == "FishCollider")
        {
            Entity entity = GameLogic.instance.getEntity(gameObject);

            if (!(entity is Fish)) return;

            Fish theFish = (Fish)entity;

            if (collision.IsTouching(theFish.GetColliders()[1]))
            {

                Entity targetEntity = GameLogic.instance.getEntity(collision.gameObject.transform.parent.parent.parent.gameObject);

                if (!(targetEntity is Fish)) return;

                Fish targetFish = (Fish)targetEntity;

                if (theFish.canEat(targetFish))
                {
                    theFish.onEat(targetFish);

                    GameLogic.instance.destroyEntity(targetFish);
                }

            }

        }
    }*/
}
