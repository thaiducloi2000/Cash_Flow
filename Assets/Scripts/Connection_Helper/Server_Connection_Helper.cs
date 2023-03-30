using System;
using UnityEngine.Networking;
using UnityEngine;
using System.Collections;
using System.Net;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Server_Connection_Helper : MonoBehaviour 
{
    private const string BASE_URL = "https://mobilebasedcashflowapi.herokuapp.com/api/";

    public IEnumerator Post(string endpoint, WWWForm form,string bodydata, Action<UnityWebRequest,float> callback)
    {

        using (UnityWebRequest request = UnityWebRequest.Post(BASE_URL + endpoint, form))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(bodydata);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            callback(request,request.downloadProgress);
        }
    }

    public IEnumerator Get(string endpoint, Action<UnityWebRequest,float> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(BASE_URL + endpoint))
        {
            yield return request.SendWebRequest();
            callback(request,request.downloadProgress);
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
}

