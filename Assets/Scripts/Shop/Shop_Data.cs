using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_Data : MonoBehaviour
{
    public List<Item> shopItemSO;

    public User_Data user_data;
    public Server_Connection_Helper helper;
    public List<Item> Items;
    private void Awake()
    {
        if (helper == null)
        {
            this.gameObject.AddComponent<Server_Connection_Helper>();
            helper = this.gameObject.GetComponent<Server_Connection_Helper>();
            helper.Authorization_Header = "Bearer " + user_data.data.token;
        }
        GetAllItems();
    }

    private void GetAllItems()
    {
        StartCoroutine(helper.Get("assets/user/asset-in-shop", (request, process) =>
        {
            Debug.Log(request.downloadHandler.text);
            if (Items == null)
            {
                Items = new List<Item>();
            }
            Items = helper.ParseToList<Item>(request);
        }));
    }
}
