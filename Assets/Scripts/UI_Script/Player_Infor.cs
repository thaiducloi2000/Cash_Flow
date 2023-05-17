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

    private void Start()
    {
        if (user == null)
        {
            user = GetComponentInParent<User_Data>();
        }
        user_name.text = user.data.user.NickName.ToString();
        Money.text = user.data.user.Coin.ToString();
    }
}
