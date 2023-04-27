using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class JobSelect : MonoBehaviour
{
    public GameObject attentionpanel;
    public TMP_Text textAttention;
    public TMP_InputField name_input;
    public Server_Connection_Helper helper;

    public User_Data user_data;


    void Awake()
    {
        if (helper == null)
        {
            helper = this.gameObject.AddComponent<Server_Connection_Helper>();
            helper.Authorization_Header = "Bearer " + this.user_data.data.token.ToString();
        }
    }

    public void changeScene()
    {
        if (checkName() == true)
        {
            UpdateAccount();
        }
    }
    public void OK()
    {
        attentionpanel.SetActive(false);
    }
    public void BackK()
    {
        SceneManager.LoadSceneAsync("StartScene");
    }
    public bool checkName()
    {
        if(name_input.text == "") {
            attentionpanel.SetActive(true);
            textAttention.text = "Khong duoc de o trong";
            return false;
        }else if(name_input.text.Length <= 2 || name_input.text.Length > 15)
        {
            attentionpanel.SetActive(true);
            textAttention.text = "Do dai ten phai lon hon 1 va be hon 12";
            return false;
        }else if(name_input.text == "DKLong")
        {
            attentionpanel.SetActive(true);
            textAttention.text = "Ten da duoc su dung";
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
            data.Add("Gender", "male");
            data.Add("Phone", "");
            data.Add("Email", "thaiducloi2000@gmail.com");
            data.Add("ImageUrl", "");
            parameter.Add("userId", this.user_data.data.user.UserId);
            string bodydata = JsonConvert.SerializeObject(data);
            StartCoroutine(helper.Put("users/profile", parameter, bodydata, (request, process) =>
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
                        Debug.LogError(": HTTP Error: " + request.error + " - " + request.downloadHandler.text);
                        textAttention.text = request.error;
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log(this.user_data.data.user == null);
                        this.user_data.data.user.NickName = name_input.text.ToString();
                        this.user_data.data.user.Gender = "male";
                        this.user_data.data.user.Email = "thaiducloi2000@gmail.com";
                        SceneManager.LoadSceneAsync("TestShopScene");
                        break;
                    default:
                        break;
                }
            }));
        }
        else
        {
            attentionpanel.SetActive(true);
            //TextAttention.text = "Register Unsuccess !! Please Try Again !!";
        }
    }
}
