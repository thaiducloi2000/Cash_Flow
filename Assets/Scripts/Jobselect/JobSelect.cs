using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.Networking;
using Unity.VisualScripting;
using System.Data.Common;

public class JobSelect : MonoBehaviour
{
    public GameObject attentionpanel;
    public TMP_Text textAttention;
    public TMP_InputField name_input;
    public GameObject name_title;
    public Server_Connection_Helper helper;

    public GameObject content;

    public User_Data user_data;


    void Awake()
    {
        if (helper == null)
        {
            helper = this.gameObject.AddComponent<Server_Connection_Helper>();
        }
        if (this.user_data.data != null)
        {
            helper.Authorization_Header = "Bearer " + this.user_data.data.token.ToString();
            bool hasNickname = isemptyNickname();
            name_input.gameObject.SetActive(hasNickname);
            name_title.gameObject.SetActive(hasNickname);
        }
    }

    private bool isemptyNickname()
    {
        return this.user_data.data.user.NickName == "" || this.user_data.data.user.NickName == null;
    }

    public void changeScene()
    {
        SwipeSelection selection = content.GetComponent<SwipeSelection>();
        if (this.user_data.data.user.NickName != "" && this.user_data.data.user.NickName != null)
        {
            Debug.Log(selection.job_Selected.Job_card_name);
            user_data.LastJobSelected = selection.job_Selected;
            UpdateLastJobSelected(this.user_data.data.user.lastCharacterSelected);
            SceneManager.LoadSceneAsync("TestShopScene");
        }
        else
        {
            if (checkName() == true)
            {
                UpdateAccount();
            }
        }
    }
    public void OK()
    {
        attentionpanel.SetActive(false);
    }
    public void BackK()
    {
        if(user_data.data.user.NickName != "" && user_data.data.user.NickName != null)
        {
            SceneManager.LoadSceneAsync("TestShopScene");
        }
        else
        {
            SceneManager.LoadSceneAsync("StartScene");
        }
    }
    public bool checkName()
    {
        if(name_input.text == "") {
            attentionpanel.SetActive(true);
            textAttention.text = "Tên Không Được để Trống!";
            return false;
        }else if(name_input.text.Length <= 2 || name_input.text.Length > 15)
        {
            attentionpanel.SetActive(true);
            textAttention.text = "Độ dài tên không được để trống và nhỏ hơn 12";
            return false;
        }
        return true;
    }

    public void UpdateAccount()
    {
        if (checkName() == true)
        {
            WWWForm form = new WWWForm();
            Dictionary<string, object> data = new Dictionary<string, object>();
            Dictionary<string, int> parameter = new Dictionary<string, int>();
            data.Add("NickName", name_input.text.ToString());
            data.Add("Gender", "nam");
            data.Add("Phone", "0948260825");
            data.Add("Email", "thaiducloi2000@gmail.com");
            data.Add("ImageUrl", "");
            parameter.Add("userId", this.user_data.data.user.UserId);
            string bodydata = JsonConvert.SerializeObject(data);
            StartCoroutine(helper.Put_Parameter("users/profile", parameter, bodydata, (request, process) =>
            {
                switch (request.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                        textAttention.text = request.downloadHandler.text;
                        break;
                    case UnityWebRequest.Result.DataProcessingError:
                        textAttention.text = request.downloadHandler.text;
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        textAttention.text = request.downloadHandler.text;
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log(this.user_data.data.user == null);
                        this.user_data.data.user.NickName = name_input.text.ToString();
                        this.user_data.data.user.Gender = "male";
                        this.user_data.data.user.Email = "thaiducloi2000@gmail.com";
                        UpdateLastJobSelected("Default");
                        break;
                    default:
                        break;
                }
            }));
        }
        else
        {
            attentionpanel.SetActive(true);
        }
    }

    private void UpdateLastJobSelected(string lastCharacterSelected)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        this.user_data.data.user.lastCharacterSelected = lastCharacterSelected;
        SwipeSelection selection = content.GetComponent<SwipeSelection>();
        user_data.LastJobSelected = selection.job_Selected;
        Character_Item character = Resources.Load<Character_Item>("Items/Character/" + this.user_data.data.user.lastCharacterSelected);

        data.Add("AssetId", character.ID);
        data.Add("LastJobSelected", user_data.LastJobSelected.Image_url);

        string bodydata = JsonConvert.SerializeObject(data);

        StartCoroutine(helper.Put("users/asset-last-used", bodydata, (request, process) =>
        {
            switch (request.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    textAttention.text = "Error: " + request.downloadHandler.text;
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    textAttention.text = "Error: " + request.downloadHandler.text;
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    textAttention.text = "Error: " + request.downloadHandler.text;
                    break;
                case UnityWebRequest.Result.Success:
                    SceneManager.LoadSceneAsync("TestShopScene");
                    break;
                default:
                    break;
            }
        }));
    }
}
