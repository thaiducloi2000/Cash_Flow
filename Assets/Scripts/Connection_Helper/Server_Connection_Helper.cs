using System;
using UnityEngine.Networking;
using UnityEngine;
using System.Collections;
using System.Net;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.UI;

public class Server_Connection_Helper : MonoBehaviour
{
    private const string BASE_URL = "https://mobilebasedcashflowapi.herokuapp.com/api/";
    private const string Content_Header= "application/json";
    public string Authorization_Header;

    public IEnumerator Post(string endpoint, WWWForm form,string bodydata, Action<UnityWebRequest,float> callback)
    {

        using (UnityWebRequest request = UnityWebRequest.Post(BASE_URL + endpoint, form))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(bodydata);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.SetRequestHeader("Content-Type", Content_Header);
            request.SetRequestHeader("Authorization", Authorization_Header);
            yield return request.SendWebRequest();
            callback(request,request.downloadProgress);
        }
    }

    public IEnumerator Post_Parameter(string endpoint, string parameter,string context, Action<UnityWebRequest, float> callback)
    {
        byte[] formDataBytes = Encoding.UTF8.GetBytes(parameter);
        using (UnityWebRequest request = UnityWebRequest.Post(BASE_URL + endpoint,"POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(formDataBytes);
            request.SetRequestHeader("Content-Type", context);
            request.SetRequestHeader("Authorization", Authorization_Header);
            yield return request.SendWebRequest();
            callback(request, request.downloadProgress);
        }
    }

    public IEnumerator PostAuthentication(string endpoint,WWWForm form, string bodydata, Action<UnityWebRequest, float> callback)
    {

        using (UnityWebRequest request = UnityWebRequest.Post(BASE_URL + endpoint, form))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(bodydata);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.SetRequestHeader("Content-Type", Content_Header);
            yield return request.SendWebRequest();
            callback(request, request.downloadProgress);
        }
    }

    public IEnumerator Put_Parameter(string endpoint, Dictionary<string, int> parameters, string bodydata, Action<UnityWebRequest, float> callback)
    {
        string url = BASE_URL + endpoint;

        foreach (KeyValuePair<string, int> parameter in parameters)
        {
            url += "?" + UnityWebRequest.EscapeURL(parameter.Key) + "=" + UnityWebRequest.EscapeURL(parameter.Value.ToString());
        }

        using (UnityWebRequest request = UnityWebRequest.Put(url, bodydata))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(bodydata);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.SetRequestHeader("Content-Type", Content_Header);
            request.SetRequestHeader("Authorization", Authorization_Header);
            yield return request.SendWebRequest();
            callback(request, request.downloadProgress);
        }
    }

    public IEnumerator Put_Parameter_Single(string endpoint,string parameters, string bodydata, Action<UnityWebRequest, float> callback)
    {
        string url = BASE_URL + endpoint + "/" + parameters;

        using (UnityWebRequest request = UnityWebRequest.Put(url, bodydata))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(bodydata);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.SetRequestHeader("Content-Type", Content_Header);
            request.SetRequestHeader("Authorization", Authorization_Header);
            yield return request.SendWebRequest();
            callback(request, request.downloadProgress);
        }
    }

    public IEnumerator Put(string endpoint, string bodydata, Action<UnityWebRequest, float> callback)
    {
        string url = BASE_URL + endpoint;

        using (UnityWebRequest request = UnityWebRequest.Put(url, bodydata))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(bodydata);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.SetRequestHeader("Content-Type", Content_Header);
            request.SetRequestHeader("Authorization", Authorization_Header);
            yield return request.SendWebRequest();
            callback(request, request.downloadProgress);
        }
    }

    public IEnumerator Get(string endpoint, Action<UnityWebRequest,float> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(BASE_URL + endpoint))
        {
            request.SetRequestHeader("Content-Type", Content_Header);
            request.SetRequestHeader("Authorization", Authorization_Header);
            yield return request.SendWebRequest();
            callback(request,request.downloadProgress);
        }
    }

    public IEnumerator Get_Parameter(string endpoint,string parameter,Action<UnityWebRequest, float> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(BASE_URL + endpoint+"/"+parameter))
        {
            request.SetRequestHeader("Content-Type", Content_Header);
            request.SetRequestHeader("Authorization", Authorization_Header);
            yield return request.SendWebRequest();
            callback(request, request.downloadProgress);
        }
    }


    public List<T> ParseToList<T> (UnityWebRequest request)
    {
        List<T> list = new List<T>();
        switch (request.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError(": Error: " + request.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(": HTTP Error: " + request.error);
                break;
            case UnityWebRequest.Result.Success:
                list = JsonConvert.DeserializeObject<List<T>>(request.downloadHandler.text);
                break;
            default:
                break;
        }
        return list;
    }

    public T ParseData<T>(UnityWebRequest request)
    {
        T data = default(T);
        switch (request.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                Debug.LogError(": Error: " + request.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(": HTTP Error: " + request.error);
                break;
            case UnityWebRequest.Result.Success:
                data = JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
                break;
            default:
                break;
        }
        return data;
    }

    public IEnumerator DownloadImage(string url, Action<Sprite> callback)
    {
        ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                if (texture != null)
                {
                    //Texture2D resize = ResizeTexture(texture, 1024, 756);
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0f, 0f));
                    callback(sprite);
                }
            }
            else
            {
                Debug.LogError(request.error);
            }
        }
    }
}

