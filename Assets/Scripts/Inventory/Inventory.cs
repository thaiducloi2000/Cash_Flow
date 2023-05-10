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

    private void Start()
    {
        GenarateItem();
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

    public void GenarateItem()
    {
        pool_item = new List<GameObject>();
        User_Data data = GetComponentInParent<Inventory_Manager>().data;
        // change ti Item when have full Item;
        foreach(Character_Item character in data.Items)
        {
            GameObject item = Instantiate(Inventory_Item_Prefab, this.Content.transform);
            item.GetComponent<Inventory_Item_UI>().Avatar.sprite = character.Avatar_Image;
            item.GetComponent<Inventory_Item_UI>().Item = character;
            item.GetComponent<Inventory_Item_UI>().Player = this.Player_Avatar;
            pool_item.Add(item);
        }
    }


}
