using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopTemplate : MonoBehaviour
{
    [SerializeField] public Item item;
    public TMP_Text nameitem_inputfield;
    public TMP_Text price_inputfield;
    public Image image;

    public void SetItem(Item item)
    {
        this.item = item;
    }

    public void Purchase()
    {
        User_Data user = this.gameObject.GetComponentInParent<Shop_Data>().user_data;
        if (user.data.user.Coin >= this.item.ItemPrice)
        {
            this.gameObject.SetActive(false);
        }
        else 
        {
            this.gameObject.SetActive(false);
        }
    }
}
