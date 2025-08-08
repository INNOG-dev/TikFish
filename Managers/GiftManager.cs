using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftManager
{
    private Dictionary<Gift.GiftData, List<Action<Gift>>> giftListeners = new Dictionary<Gift.GiftData, List<Action<Gift>>>();

    public void registerGiftListener(int giftId, Action<Gift> action)
    {
        Gift.GiftData giftData = new Gift.GiftData();

        giftData.giftId = giftId;

        List<Action<Gift>> listeners;

        if(giftListeners.ContainsKey(giftData))
        {
            listeners = giftListeners[giftData];
        }
        else
        {
            listeners = new List<Action<Gift>>();
        }

        listeners.Add(action);

        giftListeners.Add(giftData, listeners);
    }

    public void listen(Gift gift)
    {
        if(giftListeners.ContainsKey(gift.data))
        {
            Debug.Log("Contains");
            List<Action<Gift>> listeners = giftListeners[gift.data];

            listeners.ForEach(x => x(gift));
        }
        else
        {
            Fish fish = GameLogic.instance.getPlayer(gift.user);

            if (fish != null)
            {
                Debug.Log(gift.data.giftName + " value : " + gift.data.value);
                fish.addScore(gift.data.value / 4F);
            }
        }
    }

}
