using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class JobSelect : MonoBehaviour
{
    public GameObject attentionpanel;
    public TMP_Text textAttention;
    public TMP_InputField name_input;

    public void changeScene()
    {
        if (checkName() == true)
        {
            SceneManager.LoadScene("TestShopScene");
        }
    }
    public void OK()
    {
        attentionpanel.SetActive(false);
    }
    public void BackK()
    {
        SceneManager.LoadScene("StartScene");
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
}
