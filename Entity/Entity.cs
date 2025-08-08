using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Entity
{

    protected GameObject gameObject;

    protected Collider2D[] colliders;

    protected Vector2 velocity;

    protected Vector2 additionalVelocity = Vector2.one;

    protected bool isDead;

    protected System.Random rand = new System.Random();

    public abstract void Update();

    public virtual void setPosition(Vector2 position)
    {
        gameObject.transform.position = position;
    }

    public virtual Vector2 getPosition()
    {
        if (gameObject == null) return Vector2.zero;

        return gameObject.transform.position;
    }

    public virtual void Init(GameObject model, string id)
    {
        gameObject = model;
        gameObject.name = id;

        InitRenderer();

        InitColliders();
    }

    protected virtual void InitRenderer()
    {

    }

    protected virtual void InitColliders()
    {
        colliders = gameObject.transform.GetChild(0).Find("Colliders").GetComponentsInChildren<Collider2D>();
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public Collider2D[] GetColliders()
    {
        return colliders;
    }

    public Vector2 getVelocity()
    {
        return velocity;
    }

    public void setVelocity(Vector2 velocity)
    {
        this.velocity = velocity;
    }

    public void setAdditionnalVelocity(Vector2 velocity)
    {
        this.additionalVelocity = velocity;
    }

    public bool isAlive()
    {
        return !isDead;
    }

    public abstract void onDestroy();

    public virtual void OnDrawGizmos()
    {

    }

    public void setDead(bool state)
    {
        isDead = state;
    }

    public float getDistance(Entity entity)
    {
        return Vector2.Distance(entity.getPosition(), getPosition());
    }

    public float getSqrDistance(Entity entity)
    {
        Vector2 distance = entity.getPosition() - getPosition();
        return Vector2.SqrMagnitude(distance);
    }


}
