using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
using Newtonsoft.Json;
using UnityEngine.Networking;

public class Register : MonoBehaviour
{
    public TMP_InputField user_inputfield;
    public TMP_InputField pass_inputfield;
    public TMP_InputField email_inputfield;
    public TMP_InputField confirmpass_inputfield;
    public GameObject attention;
    public GameObject attentionsuccess;
    public GameObject loginpanel;
    public GameObject registerpanel;
    public TMP_Text TextAttention;
    public TMP_Text TextAttentionsuccess;
    public Server_Connection_Helper helper;

    // Start is called before the first frame update
    void Awake()
    {
        if (helper == null)
        {
            helper = GetComponentInParent<Server_Connection_Helper>();
        }
    }
    public void ToggelSelected()
    {
        PlayerPrefs.SetInt("ToggelSelected", 0);
    }
    public void ToggelSelected2()
    {
        PlayerPrefs.SetInt("ToggelSelected", 1);
    }
    public bool Check()
    {
        if (user_inputfield.text == null||pass_inputfield.text == null||email_inputfield.text == null || confirmpass_inputfield.text == null)
        {
            attention.SetActive(true);
            TextAttention.text = "Khong o nao duoc bo trong";
            return false;
        }
        if (user_inputfield.text.Length>30 && user_inputfield.text.Length<6)
        {
            attention.SetActive(true);
            TextAttention.text = "Ten dang nhap khong duoc be hon 6 va lon hon 30";
            return false;
        }
        //if (phone_inputfield.text.Length <= 11)
        //{
        //    attention.SetActive(true);
        //    TextAttention.text = "SDT phai co 11 so";
        //    return false;
        //}
        if (pass_inputfield.text.Length<8)
        {
            attention.SetActive(true);
            TextAttention.text = "Mat khau khong duoc be hon 8";
            return false;
        }
        if (confirmpass_inputfield.text != pass_inputfield.text)
        {
            attention.SetActive(true);
            TextAttention.text = "Xac nhan mat khau khong trung khop";
            return false;
        }
        return true;
    }
    public void cancle()
    {
        loginpanel.SetActive(true);
        registerpanel.SetActive(false);
    }
    public void RegisterAccount()
    {
        if (Check() == true)
        {
            WWWForm form = new WWWForm();
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("UserName", user_inputfield.text.ToString());
            data.Add("Password", pass_inputfield.text.ToString());
            data.Add("ConfirmPassword", confirmpass_inputfield.text.ToString());
            data.Add("Email", email_inputfield.text.ToString());
            string bodydata = JsonConvert.SerializeObject(data);
            StartCoroutine(helper.PostAuthentication("users/register", form, bodydata, (request, process) =>
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
                        TextAttention.text = request.error;
                        break;
                    case UnityWebRequest.Result.Success:
                        attentionsuccess.SetActive(true);
                        TextAttentionsuccess.text = "Tao tai khoan thanh cong";
                        break;
                    default:
                        break;
                }
            }));
        }
        else
        {
            attention.SetActive(true);
            //TextAttention.text = "Register Unsuccess !! Please Try Again !!";
        }
    }
}
