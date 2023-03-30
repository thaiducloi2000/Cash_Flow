using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public ShopItemSO[] shopItemSO;
    public ShopChessSO[] shopChessSO;
    public ShopTemplate[] shopTemplate;
    public GameObject[] shopPanel;
    public GameObject Attention;
    public Button[] purchase;
    public string tab;
    public int coins;
    public TMP_Text coinUI;
    private void Awake()
    {
        tab = "ChessBoardTab";
        coins = 0;
    }
    void Update()
    {
        coins = int.Parse(coinUI.text);
        if (tab == "ChessBoardTab")
        {
            for (int i = 0; i < shopChessSO.Length; i++)
                shopPanel[i].SetActive(true);
            loadTemplate();
        }
        else
        {
            for (int i = 0; i < shopItemSO.Length; i++)
                shopPanel[i].SetActive(true);
            loadTemplate();
        }
        CheckPurchase();

    }

    public void loadTemplate()
    {
        if( tab== "ChessBoardTab")
        {
            for (int i = 0; i < shopChessSO.Length; i++)
            {
                shopTemplate[i].nameitem_inputfield.text = shopChessSO[i].name;
                shopTemplate[i].price_inputfield.text = shopChessSO[i].price.ToString();
                shopTemplate[i].image.sprite = shopChessSO[i].image;
            }
        }
        else
        {
            for (int i = 0; i < shopItemSO.Length; i++)
            {
                shopTemplate[i].nameitem_inputfield.text = shopItemSO[i].name;
                shopTemplate[i].price_inputfield.text = shopItemSO[i].price.ToString();
                shopTemplate[i].image.sprite = shopItemSO[i].image;
            }
        }

    }
    public void CheckPurchase()
    {
        if (tab == "ChessBoardTab")
        {
            for(int i = 0; i < shopChessSO.Length; i++)
            {
                if(coins >= shopChessSO[i].price)
                    purchase[i].interactable=true;
                
                else
                    purchase[i].interactable = false;
            }
        }
        else
        {
            for (int i = 0; i < shopItemSO.Length; i++)
            {
                if (coins >= shopItemSO[i].price)
                    purchase[i].interactable = true;       
                else      
                    purchase[i].interactable = false;
                
            }
        }
    }
    public void buy(int btnNO)
    {
        if (coins >= shopChessSO[btnNO].price)
        {
            coins -= shopChessSO[btnNO].price;
            coinUI.text = coins.ToString();
            CheckPurchase();
        }
    }
    //public void showAttention()
    //{
    //    Attention.SetActive(true);
    //}
    public void ChessBoardTab()
    {
        tab = "ChessBoardTab";
    }
    public void OutfitTab()
    {
        tab = "OutfitTab";
    }
}
