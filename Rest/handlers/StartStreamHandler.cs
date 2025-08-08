using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStreamHandler : Handler
{
    public override object decodeJson(string json)
    {
        return null;
    }

    public override void handle(string result, Streamer user)
    {
        if(result == "ok")
        {
            StreamManager.instance.onLiveStart();
        }
        else
        {
            GameLogic.instance.dialogBox.DisplayDialogBox(result);
        }
    }
}
