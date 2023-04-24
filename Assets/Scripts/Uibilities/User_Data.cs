using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "User_Data", menuName = "User Data/User", order = 1)]
public class User_Data : ScriptableObject
{
    public Users data;
    public Character_Item Last_Character_Selected;
    public List<Character_Item> Items;
}
