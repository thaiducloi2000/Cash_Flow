using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game_Data", menuName = "Game Data/data", order = 1)]
// Storage all asset infor from db
public class Game_Data : ScriptableObject
{
    public List<Dream> dreams;
    [SerializeField] public List<Event_card_Entity> event_cards;
    [SerializeField] public List<Job> jobs;
    public List<Character_Item> characters;
}
