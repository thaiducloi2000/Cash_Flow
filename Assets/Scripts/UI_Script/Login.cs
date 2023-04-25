using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using UnityEngine.UI;
using System.Collections;

public class Login : MonoBehaviour
{
    public TMP_InputField user_inputfield;
    public TMP_InputField password_inputfield;
    public GameObject Loginpanel;
    public GameObject Regiserpanel;
    public GameObject Attentionpanel;
    public GameObject Loading_Panel;

    public Slider bar;

    public TMP_Text TextAttention;
    public Server_Connection_Helper helper;
    public User_Data user_data;

    private void Start()
    {
        Loading_Panel.gameObject.SetActive(false);
        if (helper == null)
        {
            helper = GetComponentInParent<Server_Connection_Helper>();
        }
    }
    public void SwitchScene()
    {
        Loginpanel.SetActive(false);
        Regiserpanel.SetActive(true);
    }

    public void checkLogin()
    {
        GetUserProfile(user_inputfield.text, password_inputfield.text,false);
    }

    private void GetUserProfile(string username,string password,bool isRememberMe)
    {
        WWWForm form = new WWWForm();
        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add("UserName", username);
        data.Add("Password", password);
        data.Add("RememberMe", isRememberMe);
        string bodydata = JsonConvert.SerializeObject(data);
        StartCoroutine(helper.Post("users/authenticate", form, bodydata, (request, process) =>
        {
            switch(request.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError(": Error: "+ request.error);
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(": Error: " + request.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    //Debug.LogError(": HTTP Error: " + request.error + " - " + request.downloadHandler.text);
                    Attentionpanel.SetActive(true);
                    TextAttention.text = "HTTP Error: " + request.error + " - " + request.downloadHandler.text;
                    break;
                case UnityWebRequest.Result.Success:
                    //Debug.Log(request.downloadHandler.text);
                    Users user = helper.ParseData<Users>(request);
                    helper.Authorization_Header = "Bearer " + user.token;
                    this.user_data.data = user;
                    StartCoroutine(Loading_Scene());
                    break;
                default:
                    break;
            }
        }));
    }

    public IEnumerator Loading_Scene()
    {
        Loading_Panel.SetActive(true);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("TestShopScene");
        float progress = 0;
        while (!asyncOperation.isDone)
        {
            progress = Mathf.MoveTowards(progress,asyncOperation.progress,Time.deltaTime);
            bar.value = progress;
            if(progress >= 0.9f)
            {
                bar.value = 1;
                asyncOperation.allowSceneActivation = true;
            }
            yield return new WaitForEndOfFrame();
        }

        Loading_Panel.SetActive(false);
    }
}
