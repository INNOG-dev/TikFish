using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionHandler : MonoBehaviour
{

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {

    }


    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    public virtual void onEatean(Fish fish)
    {

    }

}
