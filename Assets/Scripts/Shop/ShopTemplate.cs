using TMPro;
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

    public void SetItem(Item item,Sprite outfit_image)
    {
        this.item = item;
        this.nameitem_inputfield.text = item.AssetName;
        this.price_inputfield.text = item.AssetPrice.ToString();
        this.image.sprite = outfit_image;
    }

    public void Purchase()
    {
        if (this.user.data.user.Coin >= this.item.AssetPrice)
        {
            Server_Connection_Helper helper = GetComponentInParent<Server_Connection_Helper>();
            string parameter = "assetId=" +this.item.AssetId.ToString();
            string context = "application/x-www-form-urlencoded";
            StartCoroutine(helper.Post_Parameter("users/buy", parameter,context, (request, process) =>
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
                        Debug.LogError(request.downloadHandler.text);
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
            this.gameObject.SetActive(true);
        }
    }
}
