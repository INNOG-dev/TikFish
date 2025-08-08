using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Models
{

    public static GameObject CLOWNFISH_MODEL;
    public static GameObject FISH_MODEL;
    public static GameObject SEAL_MODEL;
    public static GameObject SHARK_MODEL;
    public static GameObject SKELETONSHARK_MODEL;
    public static GameObject SHIELD_BONUS_MODEL;
    public static GameObject STAR_BONUS_MODEL;


    public static void registerModels()
    {
        CLOWNFISH_MODEL = register("Models/Fish/ClownFish");
        FISH_MODEL = register("Models/Fish/Fish");
        SEAL_MODEL = register("Models/Fish/Seal");
        SHARK_MODEL = register("Models/Fish/Shark");
        SKELETONSHARK_MODEL = register("Models/Fish/SkeletonShark");
        SHIELD_BONUS_MODEL = register("Models/Bonus/ShieldBonus");
        STAR_BONUS_MODEL = register("Models/Bonus/StarBonus");
    }

    public static GameObject register(string path)
    {
        return Resources.Load<GameObject>(path);
    }


}
