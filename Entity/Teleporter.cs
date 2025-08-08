using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Vector2 teleportTo;

    private List<Fish> fishTriggered = new List<Fish>();

    private Collider2D collider;

    public void Awake()
    {
        collider = GetComponent<Collider2D>();
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        Entity entity = GameLogic.instance.getEntity(collision.gameObject.transform.parent.parent.parent.gameObject);

        if (collision.gameObject.name != "FishCollider") return;


        if (entity is Fish)
        {
            Fish fish = (Fish)entity;

            if (fish.getIsTeleported())
            {
                fish.setTeleported(false);
                return;
            }

            if (fish != null)
            {
                if (!fishTriggered.Contains(fish)) fishTriggered.Add(fish);
            }
        }

    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        Entity entity = GameLogic.instance.getEntity(collision.gameObject.transform.parent.parent.parent.gameObject);

        if (entity is Fish)
        {
            Fish fish = (Fish)entity;

            if (fish != null)
            {
                fishTriggered.Remove(fish);
            }

            if (collider.bounds.max.x < GameLogic.instance.getMapData().getMap().transform.position.x)
            {
                if (collision.bounds.max.x < collider.bounds.min.x)
                {
                    fish.setTeleported(true);
                    fish.setPosition(new Vector2(teleportTo.x, fish.getPosition().y));
                }
            }
            else if (collider.bounds.max.x > GameLogic.instance.getMapData().getMap().transform.position.x)
            {
                if (collision.bounds.max.x > collider.bounds.max.x)
                {
                    fish.setTeleported(true);
                    fish.setPosition(new Vector2(teleportTo.x, fish.getPosition().y));
                }
            }

        }

    }

    public void Update()
    {
        List<Fish> toRemove = new List<Fish>();

        foreach (Fish fish in fishTriggered)
        {

            if(fish.GetGameObject() == null)
            {
                toRemove.Add(fish);
            }

            if (GameUtils.IsColliderInsideAnother(fish.GetColliders()[0], collider))
            {
                fish.setTeleported(true);
                fish.setPosition(new Vector2(teleportTo.x, fish.getPosition().y));
            }
        }

        toRemove.ForEach(x => fishTriggered.Remove(x));
    }

}
