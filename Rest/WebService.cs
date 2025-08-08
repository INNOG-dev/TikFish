using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebService
{

    private static string webServiceUrl = "http://127.0.0.1:1975";

    private static string databaseUrl = "http://127.0.0.1/index.php";


    private Dictionary<Type, Handler> listeners = new Dictionary<Type, Handler>();

    private Streamer user;

    public WebService()
    {
        listeners.Add(typeof(MessageHandler), new MessageHandler());
        listeners.Add(typeof(StartStreamHandler), new StartStreamHandler());
        listeners.Add(typeof(FollowHandler), new FollowHandler());
        listeners.Add(typeof(GiftsHandler), new GiftsHandler());
        listeners.Add(typeof(LikesHandler), new LikesHandler());

    }

    public WebService setStreamer(Streamer streamer)
    {
        this.user = streamer;
        return this;
    }

    public void getLiveMessages()
    {
        RequestBuilder request = new RequestBuilder().setServiceUrl(webServiceUrl + "/messages/" + user.getTiktokName())
            .setRequestType(RequestBuilder.RequestType.GET)
            .setCallback((string result) => listeners[typeof(MessageHandler)].handle(result, user))
            .build();

        request.sendRequest();
    }

    public void getNewFollowers()
    {
        RequestBuilder request = new RequestBuilder().setServiceUrl(webServiceUrl + "/follow/" + user.getTiktokName())
            .setRequestType(RequestBuilder.RequestType.GET)
            .setCallback((string result) => listeners[typeof(FollowHandler)].handle(result, user))
            .build();

        request.sendRequest();
    }

    public void getGifts()
    {
        RequestBuilder request = new RequestBuilder().setServiceUrl(webServiceUrl + "/gift/" + user.getTiktokName())
            .setRequestType(RequestBuilder.RequestType.GET)
            .setCallback((string result) => listeners[typeof(GiftsHandler)].handle(result, user))
            .build();

        request.sendRequest();
    }

    public void getLikes()
    {
        RequestBuilder request = new RequestBuilder().setServiceUrl(webServiceUrl + "/like/" + user.getTiktokName())
            .setRequestType(RequestBuilder.RequestType.GET)
            .setCallback((string result) => listeners[typeof(LikesHandler)].handle(result, user))
            .build();

        request.sendRequest();
    }

    public void startStream()
    {
        RequestBuilder request = new RequestBuilder()
            .setServiceUrl(webServiceUrl + "/start/" + user.getTiktokName())
            .setRequestType(RequestBuilder.RequestType.GET).setCallback((string result) => listeners[typeof(StartStreamHandler)].handle(result, user))
            .build();

        request.sendRequest();
    }

    public void stopStream()
    {
        RequestBuilder request = new RequestBuilder()
            .setServiceUrl(webServiceUrl + "/stop/" + user.getTiktokName())
            .setRequestType(RequestBuilder.RequestType.GET).setCallback(null)
            .build();

        request.sendRequest();
    }

    public void addPoints(List<Tuple<string, int>> winners)
    {
        string json = JsonConvert.SerializeObject(winners);

        RequestBuilder request = new RequestBuilder()
            .setServiceUrl(databaseUrl)
            .setRequestType(RequestBuilder.RequestType.POST)
            .pushParam("winners", json)
            .setCallback(null)
            .build();

        request.sendRequest();
    }


}
