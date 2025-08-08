using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHandler : Handler
{
    public override object decodeJson(string json)
    {
        JObject jsonObj = JObject.Parse(json);
        List<TiktokUser> follows = new List<TiktokUser>();
        foreach (var property in jsonObj.Properties())
        {
            if (property.Value.Type == JTokenType.Object)
            {

                var followData = property.Value;

                var username = followData["user"]["uniqueId"].ToString();
                var userId = followData["user"]["userId"].ToString();

                TiktokUser user = new TiktokUser(username, userId);

                follows.Add(user);
            }
        }
        return follows;
    }

    public override void handle(string result, Streamer user)
    {
        if (result != "{}") Debug.Log(result);

        List<TiktokUser> followers = (List<TiktokUser>)decodeJson(result);

        foreach (TiktokUser follow in followers)
        {
            Fish fish = GameLogic.instance.getPlayer(follow);
            if(fish != null) fish.addScore(100);
        }
    }
}
