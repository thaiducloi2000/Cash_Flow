using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    public TMP_InputField user_inputfield;
    public TMP_InputField pass_inputfield;
    public TMP_InputField email_inputfield;
    public TMP_InputField name_inputfield;
    public TMP_InputField phone_inputfield;
    public GameObject attention;
    public GameObject attentionsuccess;
    public TMP_Text TextAttention;
    public TMP_Text TextAttentionsuccess;
    [SerializeField] private Toggle MaleCB;
    [SerializeField] private Toggle FemaleCB;

    // Start is called before the first frame update
    void Awake()
    {
        if (PlayerPrefs.GetInt("ToggelSelected") == 0)
        {
            MaleCB.isOn = true;
            FemaleCB.isOn = false;
        } else if (PlayerPrefs.GetInt("ToggelSelected") == 1)
        {
            MaleCB.isOn = false;
            FemaleCB.isOn = true;
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
        if (user_inputfield == null||pass_inputfield == null||email_inputfield==null||phone_inputfield==null
            || name_inputfield == null)
        {
            attention.SetActive(true);
            TextAttention.text = "Khong o nao duoc bo trong";
        }else if (user_inputfield.text.Length>30 && user_inputfield.text.Length<6)
        {
            attention.SetActive(true);
            TextAttention.text = "Username khong duoc be hon 6 va lon hon 30";
        }else if (phone_inputfield.text.Length!=10)
        {
            attention.SetActive(true);
            TextAttention.text = "SDT phai co 11 so";
        }else if (pass_inputfield.text.Length<8)
        {
            attention.SetActive(true);
            TextAttention.text = "Mat khau khong duoc be hon 8";
        }else if (name_inputfield.text.Length < 1)
        {
            attention.SetActive(true);
            TextAttention.text = "Ten khong duoc be hon 1";
        }
        else
        {
            return true;
        }
        return false;
    }
    public void RegisterAccount()
    {
        if (Check()==true)
        {
            attentionsuccess.SetActive(true);
            TextAttentionsuccess.text = "Tao tai khoan thanh cong";
        }
        else
        {
            attention.SetActive(true);
            TextAttention.text = "DCMMM";
        }
    }
}
