using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityRegistry
{
    private Dictionary<int, GameObject> registriesModel = new Dictionary<int, GameObject>();
    private Dictionary<int, Type> registriesType = new Dictionary<int, Type>();

    private Dictionary<int, string> registriesNames = new Dictionary<int, string>();

    public int register(int id, string name, Type entityType, GameObject gameObject)
    {
        if(registriesModel.ContainsKey(id))
        {
            throw new System.Exception("Id already registered");
        }

        registriesModel.Add(id, gameObject);
        registriesType.Add(id, entityType);
        registriesNames.Add(id, name);
        Debug.Log("Entity with name " + name + " id " + id + " registered");

        return id;
    }

    public Entity newEntity(int entityId, int uid)
    {
        return newEntity(entityId, uid, null);
    }

    public Entity newEntity(int entityId, int uid, params object[] parameters)
    {
        if (registriesType.ContainsKey(entityId))
        {
            Entity entity = (Entity)Activator.CreateInstance(registriesType[entityId], parameters);

            entity.Init(GameLogic.Instantiate(getModel(entityId), GameLogic.instance.transform), registriesNames[entityId] + "-" + uid);

            return entity;
        }

        return null;
    }

    public GameObject getModel(int id)
    {
        if(!registriesModel.ContainsKey(id))
        {
            Debug.Log("entity with id " + id + " not registered");
            return null;
        }
        return registriesModel[id];
    }

    public Type getEntityType(int entityId)
    {
        if(registriesType.ContainsKey(entityId))
        {
            return registriesType[entityId];
        }

        return null;
    }




}
