using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftRegistry
{

    public static void registerGifts()
    {
        GiftManager giftManager = GameLogic.instance.getGiftManager();

        giftManager.registerGiftListener(8826, (Gift gift) =>
        {
            Fish fish = GameLogic.instance.getPlayer(gift.user);

            if (fish == null) return;

            fish.AddEffect(new Effect(Effects.STAR_EFFECT), 20F);
        });

        giftManager.registerGiftListener(6070, (Gift gift) =>
        {
            Fish fish = GameLogic.instance.getPlayer(gift.user);

            if (fish == null) return;

            fish.AddEffect(new Effect(Effects.INVINCIBLE_EFFECT), 30F);
        });

        giftManager.registerGiftListener(5269, (Gift gift) =>
        {
            Fish fish = GameLogic.instance.getPlayer(gift.user);

            if (fish == null) return;

            fish.AddEffect(new Effect(Effects.INVINCIBLE_EFFECT), 30F);
        });
    }

}
