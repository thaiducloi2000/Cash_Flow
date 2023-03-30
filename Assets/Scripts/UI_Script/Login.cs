using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
//using System.IdentityModel.Tokens.Jwt;
using System.Linq;

public class Login : MonoBehaviour
{
    public TMP_InputField user_inputfield;
    public TMP_InputField password_inputfield;
    public GameObject Loginpanel;
    public GameObject Regiserpanel;
    public GameObject Attentionpanel;
    public TMP_Text TextAttention;
    public Server_Connection_Helper helper;

    private void Start()
    {
        if(helper == null)
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
        StartCoroutine(helper.Post("Users/authenticate", form, bodydata, (request, process) =>
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
                    Debug.LogError(": HTTP Error: " + request.error + " - " + request.downloadHandler.text);
                    Attentionpanel.SetActive(true);
                    TextAttention.text = "DCMMM";
                    break;
                case UnityWebRequest.Result.Success:
                    //Debug.Log(request.downloadHandler.text);
                    ParsePlayerData(request.downloadHandler.text);
                    SceneManager.LoadScene("GamePlay_Scene");
                    break;
                default:
                    break;
            }
        }));

    }


    private void ParsePlayerData(string token)
    {
        //string secrectkey = "1FDBA0CCB3E0E1DBA9885062DB7EADFF";

        //JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        //// Decode the JWT string
        //JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);

        //string name = jwtToken.Claims.FirstOrDefault(c => c.Type == "NickName")?.Value;
        //string UserName = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserName")?.Value;
        //string Id = jwtToken.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

        //Debug.Log("Nick Name: " + name);
        //Debug.Log("User Name: " + UserName);
        //Debug.Log("ID: " + Id);

    }
}
