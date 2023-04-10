using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public ShopItemSO[] shopItemSO;
    //public List<GameObject> shopItemSO;
    public ShopChessSO[] shopChessSO;
    public List<ShopTemplate> shopTemplate;
    public List<GameObject> shopPanel;
    public GameObject Attention;
    public string tab;

    //Content where prefab of template ShopItemSO spawn
    public GameObject Content;
    // Prefabs to spawn
    public GameObject ShopItemPrefab;
    //public int coins;
    public TMP_Text coinUI;
    private Shop_Data shop_data;

    private void Awake()
    {
        if(shop_data == null)
        {
            shop_data = GetComponentInParent<Shop_Data>();
        }
        SpawnItems();
        tab = "OutfitTab";
        //coins = 0;
    }
    void Update()
    {
        //coins = int.Parse(coinUI.text);
        if (tab == "OutfitTab")
        {
            for (int i = 0; i < shopPanel.Count-1; i++)
                shopPanel[i].SetActive(true);
            loadTemplate();
        }
        else
        {
            for (int i = 0; i < shopPanel.Count - 1; i++)
                shopPanel[i].SetActive(true);
            loadTemplate();
        }
        //CheckPurchase();
    }



    public void loadTemplate()
    {
        if( tab== "ChessBoardTab")
        {
            for (int i = 0; i < shopTemplate.Count; i++)
            {
                shopPanel[i].gameObject.SetActive(true);
                shopTemplate[i].nameitem_inputfield.text = shopChessSO[i].name;
                shopTemplate[i].price_inputfield.text = shopChessSO[i].price.ToString();
                shopTemplate[i].image.sprite = shopChessSO[i].image;
            }
        }
        else
        {
            for (int i = 0; i < shopTemplate.Count; i++)
            {
                shopPanel[i].gameObject.SetActive(true);
                shopTemplate[i].nameitem_inputfield.text = shopItemSO[i].name;
                shopTemplate[i].price_inputfield.text = shopItemSO[i].price.ToString();
                shopTemplate[i].image.sprite = shopItemSO[i].image;
            }
        }

    }
    //public void CheckPurchase()
    //{
    //    if (tab == "ChessBoardTab")
    //    {
    //        for(int i = 0; i < shopChessSO.Length; i++)
    //        {
    //            if(shop_data.user_data.data.user.Coin >= shopChessSO[i].price)
    //                purchase[i].interactable=true;
                
    //            else
    //                purchase[i].interactable = false;
    //        }
    //    }
    //    else
    //    {
    //        for (int i = 0; i < shopItemSO.Count-1; i++)
    //        {
    //            if (shop_data.user_data.data.user.Coin >= shopItemSO[i].GetComponent<ShopItemSO>().price)
    //                purchase[i].interactable = true;       
    //            else      
    //                purchase[i].interactable = false;
                
    //        }
    //    }
    //}
    
    public void SpawnItems()
    {
        shopPanel = new List<GameObject>();
        shopTemplate = new List<ShopTemplate>();
        for (int i = 0;i< shop_data.Items.Count; i++)
        {
            GameObject item = Instantiate(ShopItemPrefab, Content.transform);
            shopItemSO[i].price = shop_data.Items[i].ItemPrice;
            shopItemSO[i].name = shop_data.Items[i].ItemName;
            shopPanel.Add(item);
            shopTemplate.Add(item.GetComponent<ShopTemplate>());
        }
        foreach(GameObject item in shopPanel)
        {
            item.SetActive(false);
        }
    }

    public void buy(int btnNO)
    {
        if (shop_data.user_data.data.user.Coin >= shopChessSO[btnNO].price)
        {
            shop_data.user_data.data.user.Coin -= shopChessSO[btnNO].price;
            coinUI.text = shop_data.user_data.data.user.Coin.ToString();
            //CheckPurchase();
        }
    }

    public void ChessBoardTab()
    {
        tab = "ChessBoardTab";
    }
    public void OutfitTab()
    {
        tab = "OutfitTab";
    }
}
