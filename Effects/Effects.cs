using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects
{

    public static EffectProperty INVINCIBLE_EFFECT;
    public static EffectProperty STAR_EFFECT;


    public static void registerEffects()
    {
        EffectRegistry registry = GameLogic.getEffectRegistry();
        INVINCIBLE_EFFECT = registry.register(0, 15, 1, typeof(ShieldEffect), "Invincible");
        STAR_EFFECT = registry.register(1, 10, 1, typeof(StarEffect), "Star");

    }

}
