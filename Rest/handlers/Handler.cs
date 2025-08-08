using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Handler
{

     public abstract void handle(string result, Streamer user);

    public abstract object decodeJson(string json);
}
