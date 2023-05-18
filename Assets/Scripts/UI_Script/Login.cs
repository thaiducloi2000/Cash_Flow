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
    public Game_Data game_data;

    private bool isDownloading = false;

    private void Awake()
    {
        Loading_Panel.gameObject.SetActive(false);
        game_data = Resources.Load<Game_Data>("Items/Game_Data");
        if (helper == null)
        {
            helper = GetComponentInParent<Server_Connection_Helper>();
        }
    }

    private void Start()
    {
        isDownloading = true;
        Loading_Data();
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
                    TextAttention.text = "Error: Connection Error";
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    TextAttention.text = "Error: Data Processing Error";
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Attentionpanel.SetActive(true);
                    TextAttention.text = "Error: Protocol Error";
                    break;
                case UnityWebRequest.Result.Success:
                    Users user = helper.ParseData<Users>(request);
                    if(user.user.lastCharacterSelected == null)
                    {
                        user.user.lastCharacterSelected = "Default";
                    }
                    helper.Authorization_Header = "Bearer " + user.token;
                    this.user_data.data = user;
                    this.user_data.Last_Character_Selected = Resources.Load<Character_Item>("Items/"+ user.user.lastCharacterSelected);
                    foreach (Job job in game_data.Jobs)
                    {
                        if (job.Image_url == user.user.LastJobSelected)
                        {
                            Debug.Log(job.Image_url + " " + job.Job_card_name);
                            user_data.LastJobSelected = job;
                        }
                    }
                    if (user_data.data.user.NickName != null && user_data.data.user.NickName != "")
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

    private void Loading_Data()
    {

        StartCoroutine(helper.Get("jobcards/all", (request, process) =>
        {
            this.game_data.Jobs = new List<Job>();
            List<Job> jobs = helper.ParseToList<Job>(request);
            foreach (Job job in jobs)
            {
                this.game_data.Jobs.Add(job);
            }
            if (this.game_data.Jobs.Count > 0)
            {
                isDownloading = false;
            }
        }));
    }

    private IEnumerator Loading_Scene(string scene)
    {
        Loading_Panel.SetActive(true);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);
        asyncOperation.allowSceneActivation = false;
        float progress = 0;
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
