using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Shoppanel;
    public void openShop()
    {
        Shoppanel.SetActive(true);
    }

    public void closeShop()
    {
        Shoppanel.SetActive(false);
    }

}
