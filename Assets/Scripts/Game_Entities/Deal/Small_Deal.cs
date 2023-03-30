
public class Small_Deal : Deal
{
    private float dept;
    private float cash_flow;
    private float downsize;
    private string trading_Range;

    public Small_Deal(string title,string Account_Name, string description, float cost, float dept, float cash_flow, float downsize,string trading_range, int Action)
    {
        this.Type = Deal_Type.Small;
        this.Title = title;
        this.Account_Name = Account_Name;
        this.Description = description;
        this.Cost = cost;
        this.dept = dept;
        this.Downsize = downsize;
        this.trading_Range = trading_range;
        this.Action = Action;
        this.cash_flow = cash_flow;
    }

    public float Dept { get => dept; set => dept = value; }
    public float Cash_flow { get => cash_flow; set => cash_flow = value; }
    public float Downsize { get => downsize; set => downsize = value; }
    public string Trading_Range { get => trading_Range; set => trading_Range = value; }
}
