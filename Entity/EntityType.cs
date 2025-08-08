using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityType
{

    public static int ENTITY_CLOWNFISH;
    public static int ENTITY_FISH;
    public static int ENTITY_SHARK;
    public static int ENTITY_SEAL;
    public static int ENTITY_SKELETONSHARK;

    public static int ENTITY_SHIELDBONUS;
    public static int ENTITY_STARBONUS;


    public static void registerEntities()
    {
        EntityRegistry registry = GameLogic.getEntityRegistry();

        ENTITY_CLOWNFISH = registry.register(0, "ClownFish", typeof(Fish), Models.CLOWNFISH_MODEL);
        ENTITY_SHIELDBONUS = registry.register(1, "Shield", typeof(Bonus), Models.SHIELD_BONUS_MODEL);
        ENTITY_STARBONUS = registry.register(2, "Star", typeof(Bonus), Models.STAR_BONUS_MODEL);
        ENTITY_FISH = registry.register(3, "Fish", typeof(Fish), Models.FISH_MODEL);
        ENTITY_SEAL = registry.register(4, "Seal", typeof(Fish), Models.SEAL_MODEL);
        ENTITY_SHARK = registry.register(5, "Shark", typeof(Fish), Models.SHARK_MODEL);
        ENTITY_SKELETONSHARK = registry.register(6, "SkeletonShark", typeof(Fish), Models.SKELETONSHARK_MODEL);
    }

    public static bool isEntityOfType(int entityId, Type type)
    {
        EntityRegistry registry = GameLogic.getEntityRegistry();

        return registry.getEntityType(entityId) == type;
    }

}
