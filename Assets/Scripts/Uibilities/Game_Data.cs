using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game_Data", menuName = "Game Data/data", order = 1)]
// Storage all asset infor from db
public class Game_Data : ScriptableObject
{
    private List<Job> jobs;
    public List<Character_Item> characters;

    public List<Job> Jobs { get => jobs; set => jobs = value; }
}
