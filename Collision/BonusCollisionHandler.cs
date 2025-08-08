using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusCollisionHandler : CollisionHandler
{

    public override void onEatean(Fish fish)
    {
        Entity entity = GameLogic.instance.getEntity(gameObject);
        if (entity is Bonus)
        {
            Bonus bonusEntity = (Bonus)entity;

            if(bonusEntity.isAlive())
            {
                bonusEntity.setDead(true);

                DelayedAction.InvokeDelayed(delegate
                {
                    fish.AddEffect(bonusEntity.GetEffect());

                    GameLogic.instance.destroyEntity(bonusEntity);
                }, 0.25F);
            }
        }
    }

    /*public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "EatCollider")
        {
            Entity entity = GameLogic.instance.getEntity(gameObject);
            if (entity is Bonus)
            {
                Bonus bonusEntity = (Bonus)entity;
                Fish fish = (Fish)GameLogic.instance.getEntity(collision.transform.parent.parent.parent.gameObject);



                fish.AddEffect(bonusEntity.GetEffect());

                GameLogic.instance.destroyEntity(bonusEntity);
            }
        }
    }*/

}
