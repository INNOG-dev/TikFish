using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LikesHandler : Handler
{
    public override object decodeJson(string json)
    {
        JObject jsonObj = JObject.Parse(json);
        List<Like> likes = new List<Like>();
        foreach (var property in jsonObj.Properties())
        {
            if (property.Value.Type == JTokenType.Object)
            {
                Like like = new Like();


                var likeData = property.Value;

                var username = likeData["user"]["uniqueId"].ToString();
                var userId = likeData["user"]["userId"].ToString();

                like.user = new TiktokUser(username, userId);
                like.count = (int)likeData["count"];

                likes.Add(like);
            }
        }
        return likes;
    }

    public override void handle(string result, Streamer user)
    {
        if (result != "{}") Debug.Log(result);

        List <Like> likes = (List<Like>)decodeJson(result);

        foreach(Like like in likes)
        {
            Fish fish = GameLogic.instance.getPlayer(like.user);
            if(fish != null)fish.addScore(like.count / 5F);
        }
    }

}
