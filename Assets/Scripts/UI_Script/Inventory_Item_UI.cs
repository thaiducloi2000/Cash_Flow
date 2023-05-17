using Newtonsoft.Json;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Inventory_Item_UI : MonoBehaviour
{
    public Image Avatar;
    public Character_Item Item;
    public GameObject Player;
    public User_Data _data;

    public void ViewItem()
    {
        Dictionary<string, object> character_selected = new Dictionary<string, object>();

        Server_Connection_Helper helper = new Server_Connection_Helper();
        helper.Authorization_Header = "Bearer " + _data.data.token;
        Debug.Log("Change avatar: "+ Item.ID);
        character_selected.Add("AssetId", Item.ID);
        character_selected.Add("LastJobSelected", _data.LastJobSelected.Image_url);
        string bodydata_1 = JsonConvert.SerializeObject(character_selected);

        StartCoroutine(helper.Put("users/asset-last-used", bodydata_1, (request, process) =>
        {
            switch (request.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log(request.downloadHandler.text);
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log(request.downloadHandler.text);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log(request.downloadHandler.text);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(request.downloadHandler.text);
                    break;
                default:
                    break;
            }
        }));
        ThirdPersonController controller = Player.GetComponent<ThirdPersonController>();
        controller.ChangeAvatar(Item);
    }
}
