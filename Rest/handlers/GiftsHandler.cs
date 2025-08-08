using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftsHandler : Handler
{
    public override object decodeJson(string json)
    {
        JObject jsonObj = JObject.Parse(json);
        List<Gift> gifts = new List<Gift>();
        foreach (var property in jsonObj.Properties())
        {
            if (property.Value.Type == JTokenType.Object)
            {
                Gift gift = new Gift();


                var giftData = property.Value;

                var username = giftData["user"]["uniqueId"].ToString();
                var userId = giftData["user"]["userId"].ToString();



                gift.user = new TiktokUser(username, userId);

                gift.data = new Gift.GiftData();
                gift.data.giftId = (int)giftData["gift"]["giftId"];
                gift.data.giftName = giftData["gift"]["giftName"].ToString();
                gift.data.count = (int) giftData["gift"]["count"];
                gift.data.value = (int)giftData["gift"]["value"];

                gifts.Add(gift);

            }
        }
        return gifts;
    }

    public override void handle(string result, Streamer user)
    {
        if(result != "{}") Debug.Log(result);

        List<Gift> gifts = (List<Gift>)decodeJson(result);

        GiftManager giftManager = GameLogic.instance.getGiftManager();

        foreach (Gift gift in gifts)
        {
            giftManager.listen(gift);
        }
    }
}
