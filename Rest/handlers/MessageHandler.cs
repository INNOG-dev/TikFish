using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageHandler : Handler
{
    public override object decodeJson(string json)
    {
        JObject jsonObj = JObject.Parse(json);
        List<Message> messages = new List<Message>();
        foreach (var property in jsonObj.Properties())
        {
            if (property.Value.Type == JTokenType.Object)
            {
                Message message = new Message();


                var messageData = property.Value;

                var username = messageData["user"]["uniqueId"].ToString();
                var userId = messageData["user"]["userId"].ToString();

                string messageStr = messageData["message"].ToString();


                message.message = messageStr;
                message.user = new TiktokUser(username, userId);

                messages.Add(message);

            }
        }
        return messages;
    }

    public override void handle(string result, Streamer user)
    {
        List<Message> messages = (List<Message>) decodeJson(result);
        foreach (Message message in messages)
        {
            int action = -1;

            if (int.TryParse(message.message, out action))
            {
                if(action <= 5)
                {
                    if (!GameLogic.instance.isInGame(message.user))
                    {
                        int entityId = 0;
                        switch(action)
                        {
                            case 0:
                            {
                                entityId = EntityType.ENTITY_FISH;
                                break;
                            }
                            case 1:
                            {
                                entityId = EntityType.ENTITY_CLOWNFISH;
                                break;
                            }
                            case 2:
                            {
                                entityId = EntityType.ENTITY_SEAL;
                                break;
                            }
                            case 3:
                            {
                                entityId = EntityType.ENTITY_SHARK;
                                break;
                            }
                            case 4:
                            {
                                entityId = EntityType.ENTITY_SKELETONSHARK;
                                break;
                            }
                        }
                        GameLogic.instance.addPlayer(message.user, entityId);
                    }
                }
                
            }
        }
    }
}
