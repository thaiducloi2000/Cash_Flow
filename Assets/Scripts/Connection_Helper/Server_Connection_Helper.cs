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
                    Texture2D resize = ResizeTexture(texture, 1024, 756);
                    Sprite sprite = Sprite.Create(resize, new Rect(0, 0, resize.width, resize.height), new Vector2(0f, 0f));
                    callback(sprite);
                }
            }
            else
            {
                Debug.LogError(request.error);
            }
        }
    }

    private Texture2D ResizeTexture(Texture2D originalTexture, int newWidth, int newHeight)
    {
        Texture2D resizedTexture = new Texture2D(newWidth, newHeight);
        Color[] pixels = new Color[newWidth * newHeight];
        float xRatio = (float)originalTexture.width / newWidth;
        float yRatio = (float)originalTexture.height / newHeight;

        for (int y = 0; y < newHeight; y++)
        {
            for (int x = 0; x < newWidth; x++)
            {
                int index = (int)(y * xRatio) * originalTexture.width + (int)(x * yRatio);
                pixels[y * newWidth + x] = originalTexture.GetPixel(index % originalTexture.width, index / originalTexture.width);
            }
        }

        resizedTexture.SetPixels(pixels);
        resizedTexture.Apply();

        return resizedTexture;
    }
}

