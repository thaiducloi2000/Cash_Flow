using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopMenu", menuName = "Scriptable Objects/New Chess Item", order = 1)]
public class ShopChessSO : ScriptableObject
{
    public string name;
    public int price;
    public Sprite image;
}
