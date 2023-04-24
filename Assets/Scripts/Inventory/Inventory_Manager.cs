using UnityEngine;

public class Inventory_Manager : MonoBehaviour
{
    public User_Data data;

    private void Awake()
    {
        this.data = GetComponentInParent<Shop_Data>().user_data;
    }
}
