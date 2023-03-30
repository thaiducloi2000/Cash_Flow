
public class Doodad : Deal
{
    // Start is called before the first frame update
    public Doodad(string title, string Account_name,string description, float cost,int Action)
    {
        base.Type = Deal_Type.Doodad;
        base.Title= title;
        this.Account_Name = Account_Name;
        base.Description= description;
        base.Cost= cost;
        base.Action = Action;
    }
}
