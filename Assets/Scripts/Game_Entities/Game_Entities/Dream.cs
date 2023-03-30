using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dream : MonoBehaviour
{
    public string id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Cost { get; set; }
    public Dream(string id, string name, string description, int cost)
    {
        this.id = id;
        Name = name;
        Description = description;
        Cost = cost;
    }
}
