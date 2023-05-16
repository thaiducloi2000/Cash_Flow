using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player_Infor : MonoBehaviour
{
    [SerializeField] public User_Data user;
    [SerializeField] public TextMeshProUGUI user_name;
    [SerializeField] public TextMeshProUGUI Money;
    [SerializeField] public Image avatar;
    Server_Connection_Helper helper;

    private void Awake()
    {
        if (user == null)
        {
            user = GetComponentInParent<User_Data>();
        }
        helper = GetComponentInParent<Server_Connection_Helper>();
    }
    private void Start()
    {
        user_name.text = user.data.user.NickName.ToString();
        Money.text = user.data.user.Coin.ToString();
    }

    private void OnApplicationQuit()
    {
        StartCoroutine(SaveDataLastUserData());
    }

    public IEnumerator SaveDataLastUserData()
    {
        bool isPutData_1 = true;
        bool isPutData_2 = true;
        Dictionary<string, object> character_selected = new Dictionary<string, object>();
        Character_Item character = Resources.Load<Character_Item>("Items/Character/" + user.data.user.lastCharacterSelected);

        character_selected.Add("AssetId", character.ID);
        character_selected.Add("LastJobSelected", user.LastJobSelected.Image_url);

        string bodydata = JsonConvert.SerializeObject(character_selected);

        StartCoroutine(helper.Put("users/asset-last-used", bodydata, (request, process) =>
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
                    isPutData_1 = false;
                    break;
                default:
                    break;
            }
        }));

        Dictionary<string, int> parameter = new Dictionary<string, int>();
        Dictionary<string, object> point_coint = new Dictionary<string, object>();
        point_coint.Add("Coin", user.data.user.Coin);
        point_coint.Add("Point", user.data.user.Point);
        parameter.Add("userId", user.data.user.UserId);
        bodydata = JsonConvert.SerializeObject(point_coint);
        StartCoroutine(helper.Put_Parameter("users/coin-point", parameter, bodydata, (request, process) =>
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
                    isPutData_2 = false;
                    break;
                default:
                    break;
            }
        }));
        yield return new WaitUntil(() => isPutData_1 == isPutData_2 == false);

        Application.Quit();
    }
}
