using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtils
{
    public static Vector2 getRandomPositionWithinBounds(Transform transform)
    {
        Renderer renderer = transform.GetComponent<Renderer>();

        Bounds bounds = renderer.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(randomX, randomY);
    }

    public static bool IsColliderInsideAnother(Collider2D inner, Collider2D outer)
    {
        if (inner == null || outer == null)
        {
            Debug.LogError("Collider2Ds are not assigned.");
            return false;
        }

        Bounds innerBounds = inner.bounds;
        Bounds outerBounds = outer.bounds;

        return outerBounds.Contains(innerBounds.min) && outerBounds.Contains(innerBounds.max);
    }
}
