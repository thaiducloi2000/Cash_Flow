using System.Collections.Generic;
using UnityEngine;
// Entity from API
public class Tile_Entity
{
    public string _id { get; set; }
    public string tile_type { get; set; }
    public List<int> positions { get; set; }

    public string race_type { get; set; }

    public string Stringify()
    {
        return JsonUtility.ToJson(this);
    }

    public static Tile_Entity Parse(string json)
    {
        return JsonUtility.FromJson<Tile_Entity>(json);
    }
}
