using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public List<Character_Item> shopItemSO;
    //public List<GameObject> shopItemSO;
    public List<ShopChessSO> shopChessSO;
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
        shopItemSO = Resources.Load<Game_Data>("Items/Game_Data").characters;
        SpawnItems();
        tab = "OutfitTab";
        coinUI.text = shop_data.user_data.data.user.Coin.ToString();
    }
    void Update()
    {

    }

    public void PopupShop()
    {
        if (tab == "OutfitTab")
        {
            for (int i = 0; i < shopPanel.Count - 1; i++)
                shopPanel[i].SetActive(true);
            loadTemplate();
        }
        else
        {
            for (int i = 0; i < shopPanel.Count - 1; i++)
                shopPanel[i].SetActive(true);
            loadTemplate();
        }
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
            for (int i = 0; i < shop_data.Items.Count; i++)
            {
                shopPanel[i].gameObject.SetActive(true);
            }
        }

    }
    
    public void SpawnItems()
    {
        shopPanel = new List<GameObject>();
        shopTemplate = new List<ShopTemplate>();
        Debug.Log(shop_data.Items.Count);
        for (int i = 0;i < shop_data.Items.Count; i++)
        {
            GameObject item = Instantiate(ShopItemPrefab, Content.transform);
            item.GetComponent<ShopTemplate>().item = shop_data.Items[i];
            foreach(Character_Item outfit in shopItemSO)
            {
                if(outfit.ID == shop_data.Items[i].AssetId.ToString())
                {
                    item.GetComponent<ShopTemplate>().SetItem(shop_data.Items[i], outfit.Avatar_Image);
                }
            }
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
