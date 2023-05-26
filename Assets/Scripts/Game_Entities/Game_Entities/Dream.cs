
using UnityEngine;

public class Dream : MonoBehaviour
{
    public string id { get; set; }
    public string Name { get; set; }
    public int Cost { get; set; }
    public bool Status { get; set; }
    public int Game_mod_id { get; set; }

    public Dream(string id, string name, int cost, bool status, int game_mod_id)
    {
        this.id = id;
        Name = name;
        Cost = cost;
        Status = status;
        Game_mod_id = game_mod_id;
    }
}
