using UnityEngine;

[CreateAssetMenu(fileName = "Character_Items", menuName = "Items/Character", order = 1)]
public class Character_Item : ScriptableObject
{
    public Item_Type item = Item_Type.Avatar;
    public string ID;
    public GameObject Prefabs;
    public Sprite Avatar_Image;
}
