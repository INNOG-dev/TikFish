using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{

    public static LayerMask obstacleMask;

    public Collider2D worldBorderUP;
    public Collider2D worldBorderDOWN;
    public Collider2D worldBorderLEFT;
    public Collider2D worldBorderRIGHT;

    private Transform map;

    public void Awake()
    {
        obstacleMask = LayerMask.GetMask("Obstacle");

        map = transform;
    }

    public Transform getMap()
    {
        return map;
    }

    public Vector2 getRandomMapPos()
    {
        return GameUtils.getRandomPositionWithinBounds(map);
    }
}
