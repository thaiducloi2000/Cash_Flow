using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_Infor : MonoBehaviour
{
    [SerializeField] public User_Data user;
    [SerializeField] public TextMeshProUGUI user_name;
    [SerializeField] public TextMeshProUGUI Money;
    [SerializeField] public Image avatar;

    //private void Awake()
    //{
    //    if(user == null)
    //    {
    //        user = GetComponentInParent<User_Data>();
    //    }
    //}
    private void Start()
    {
        user_name.text = user.data.user.NickName.ToString();
        Money.text = user.data.user.Coin.ToString();
    }
}
