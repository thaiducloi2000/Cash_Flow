using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dream_Item", menuName = "Items/Dream", order = 1)]
public class Dream_Item : ScriptableObject
{
    [Header("Property Dream")]
    public string ID;
    [Header("Model Dream")]
    public GameObject Dream_Model;
}