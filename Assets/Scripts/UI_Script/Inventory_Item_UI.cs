using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_Item_UI : MonoBehaviour
{
    public Image Avatar;
    public Character_Item Item;
    public GameObject Player;

    public void ViewItem()
    {
        ThirdPersonController controller = Player.GetComponent<ThirdPersonController>();
        controller.ChangeAvatar(Item);
    }
}
