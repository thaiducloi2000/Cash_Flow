using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject InventoryPanel;
    public GameObject Content;
    public GameObject Inventory_Item_Prefab;
    public List<GameObject> pool_item;
    public GameObject Player_Avatar;
    public Server_Connection_Helper helper;

    private void Awake()
    {
        if (helper == null)
        {
            helper = GetComponentInParent<Server_Connection_Helper>();
        }
    }

    private void Start()
    {
        Load_All_My_Assets();
    }
    public void openInven()
    {
        OpenPage("");
        InventoryPanel.SetActive(true);
    }
    public void closeInven()
    {
        InventoryPanel.SetActive(false);
    }

    public void OpenPage(string page_name)
    {
        switch (page_name)
        {
            case "Chess":
                break;
            case "Shirt":
                break;
            case "Pant":
                break;
            case "Shoe":
                break;
            case "Hair":
                break;
            default:
                break;

        }
    }

    public void Load_All_My_Assets()
    {
        User_Data data = GetComponentInParent<Inventory_Manager>().data;
        data.Items = new List<Character_Item>();
        StartCoroutine(helper.Get("users/my-asset", (request, process) =>
        {
            List<My_Asset> assets = new List<My_Asset>();
            assets = helper.ParseToList<My_Asset>(request);
            Character_Item[] items = Resources.LoadAll<Character_Item>("Items/Character");
            foreach (My_Asset asset in assets)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].ID == asset.AssetId)
                    {
                        data.Items.Add(items[i]);
                    }
                }
            }
            pool_item = new List<GameObject>();
            // change ti Item when have full Item;
            foreach (Character_Item character in data.Items)
            {
                GameObject item = Instantiate(Inventory_Item_Prefab, this.Content.transform);
                item.GetComponent<Inventory_Item_UI>().Avatar.sprite = character.Avatar_Image;
                item.GetComponent<Inventory_Item_UI>().Item = character;
                item.GetComponent<Inventory_Item_UI>().Player = this.Player_Avatar;
                item.GetComponent<Inventory_Item_UI>()._data = this.Player_Avatar.GetComponent<ThirdPersonController>()._data;
                pool_item.Add(item);
            }
        }));
    }
}
