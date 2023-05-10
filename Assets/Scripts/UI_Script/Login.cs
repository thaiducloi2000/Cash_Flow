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
    public Game_Data data;

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
                    Attentionpanel.SetActive(true);
                    TextAttention.text = "HTTP Error: " + request.error + " - " + request.downloadHandler.text;
                    break;
                case UnityWebRequest.Result.Success:
                    //Debug.Log(request.downloadHandler.text);
                    Users user = helper.ParseData<Users>(request);
                    helper.Authorization_Header = "Bearer " + user.token;
                    this.user_data.data = user;
                    if(user_data.data.user.NickName != null && user_data.data.user.NickName != "")
                    {

                        StartCoroutine(Loading_Scene("TestShopScene"));
                    }
                    else
                    {
                        StartCoroutine(Loading_Scene("CharacterSelection"));
                    }
                    break;
                default:
                    break;
            }
        }));
    }

    public IEnumerator Loading_Scene(string scene)
    {
        Loading_Panel.SetActive(true);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);
        float progress = 0;
        // Get Data
        StartCoroutine(helper.Get("eventcards/all", (request, process) =>
        {
            this.data.event_cards = helper.ParseToList<Event_card_Entity>(request);
        }));
        StartCoroutine(helper.Get("jobcards/all", (request, process) =>
        {
            this.data.jobs = helper.ParseToList<Job>(request);
        }));
        StartCoroutine(helper.Get("dreams/all", (request, process) =>
        {
            this.data.dreams = helper.ParseToList<Dream>(request);
        }));
        while (!asyncOperation.isDone)
        {
            progress = Mathf.MoveTowards(progress,asyncOperation.progress,Time.deltaTime);
            bar.value = progress * 0.25f;
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
