using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Streamer 
{
    private string tiktokName;

    private string lastGifter;

    private string lastGift;

    private bool streamer;


    public Streamer(string tiktokName, string lastGifter, string lastGift)
    {
        this.tiktokName = tiktokName;
        this.lastGift = lastGift;
        this.lastGifter = lastGifter;
        streamer = true;
    }

    public bool IsStreamer()
    {
        return streamer;
    }

    public string getLastGift()
    {
        return lastGift;
    }

    public void setLastGift(string lastGift)
    {
        this.lastGift = lastGift;
    }

    public string getLastGifter()
    {
        return lastGifter;
    }

    public void setLastGifter(string lastGifter)
    {
        this.lastGifter = lastGifter;
    }

    public string getTiktokName()
    {
        return tiktokName;
    }


}
