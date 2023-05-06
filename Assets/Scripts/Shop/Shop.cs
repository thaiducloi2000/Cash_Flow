using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Shoppanel;
    public GameObject JoyStick_Panel;

    public void openShop()
    {
        JoyStick_Panel.SetActive(false);
        Shoppanel.SetActive(true);
        Shoppanel.GetComponent<ShopManager>().PopupShop();
    }

    public void closeShop()
    {
        Shoppanel.SetActive(false);
        JoyStick_Panel.SetActive(true);
    }

}
