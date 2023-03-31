
using UnityEngine;
using UnityEngine.UI;

public class Market : Deal
{
    public Market(Sprite image_url, string title, string Account_name,string description, float cost,int Action)
    {
        base.Type = Deal_Type.Doodad;
        this.Image = image_url;
        base.Title = title;
        this.Account_Name = Account_Name;
        base.Description = description;
        base.Cost = cost;
        base.Action = Action;
    }
}
