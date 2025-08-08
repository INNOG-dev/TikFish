using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RequestBuilder
{

    private string serviceUrl = "";

    private Dictionary<string, string> headers = new Dictionary<string, string>();

    private Dictionary<string, string> parameters = new Dictionary<string, string>();

    private UnityWebRequest request;

    private Action<string> requestCallback;

    public enum RequestType
    {
        POST,
        GET
    };

    private RequestType type;

    public RequestBuilder setRequestType(RequestType type)
    {
        this.type = type;
        return this;
    }

    public RequestBuilder setCallback(Action<string> callback)
    {
        requestCallback = callback;
        return this;
    }

    public RequestBuilder build()
    {
        string url = serviceUrl.TrimEnd('/');

        if (type == RequestType.POST)
        {

            WWWForm form = new WWWForm();

            foreach (KeyValuePair<string, string> keyValuePair in parameters)
            {
                form.AddField(keyValuePair.Key, keyValuePair.Value);
            }

            request = UnityWebRequest.Post(url, form);
        }
        else
        {
            if(parameters.Count > 0)
            {
                url += "?";
                foreach (KeyValuePair<string, string> keyValuePair in parameters)
                {
                    url += keyValuePair.Key + "=" + keyValuePair.Value + "&";
                }
                url = url.TrimEnd('&');
            }

            request = UnityWebRequest.Get(url);


            foreach (KeyValuePair<string, string> keyValuePair in headers)
            {
                request.SetRequestHeader(keyValuePair.Key, keyValuePair.Value);
            }
        }
        return this;
    }

    public void sendRequest()
    {
        GameLogic.instance.LoadingGameobjectUI.SetActive(true);
        GameLogic.instance.StartCoroutine(sendRequestEnum());
    }

    private IEnumerator sendRequestEnum()
    {
        yield return request.SendWebRequest();

        if (requestCallback != null)
        {

            if (request.result != UnityWebRequest.Result.Success)
            {

                requestCallback.Invoke(request.error);
            }
            else
            {
                requestCallback.Invoke(request.downloadHandler.text);
            }
        }

        GameLogic.instance.LoadingGameobjectUI.SetActive(false);
    }


    public RequestBuilder setServiceUrl(string url)
    {
        serviceUrl = url;
        return this;
    }

    public RequestBuilder pushHeader(string name, string value)
    {
        headers.Add(name, value);
        return this;
    }

    public RequestBuilder pushParam(string key, string value)
    {
        parameters.Add(key, value);
        return this;
    }



}
