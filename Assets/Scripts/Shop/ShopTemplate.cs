using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ShopTemplate : MonoBehaviour
{
    [SerializeField] public Item item;
    public TMP_Text nameitem_inputfield;
    public TMP_Text price_inputfield;
    public Image image;
    public User_Data user;
    public void SetItem(Item item)
    {
        this.item = item;
    }

    public void Purchase()
    {
        if (this.user.data.user.Coin >= this.item.AssetPrice)
        {
            Server_Connection_Helper helper = GetComponentInParent<Server_Connection_Helper>();
            WWWForm form = new WWWForm();
            Dictionary<string, int> data = new Dictionary<string, int>();
            Debug.Log(this.item.AssetId);
            data.Add("assetId", this.item.AssetId);
            string bodydata = JsonConvert.SerializeObject(data);
            StartCoroutine(helper.Post("users/buy",form, bodydata, (request, process) =>
            {
                switch (request.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                        Debug.LogError(": Error: " + request.error);
                        break;
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError(": Error: " + request.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError(request.error);
                        break;
                    case UnityWebRequest.Result.Success:
                        //Debug.Log(request.downloadHandler.text);
                        this.gameObject.SetActive(false);
                        break;
                    default:
                        break;
                }
            }));
        }
        else 
        {
            this.gameObject.SetActive(false);
        }
    }
}
