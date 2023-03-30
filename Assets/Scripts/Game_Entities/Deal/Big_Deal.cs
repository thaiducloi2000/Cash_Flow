
public class Big_Deal : Deal
{
    private float downpay;
    private string tradingRange;
    private float dept;
    private float cash_flow;

    public Big_Deal(string title,string Account_name,string description,float cost,float downpay, string tradingRange, float dept, float cash_flow,int Action)
    {
        this.Type = Deal_Type.Big;
        this.Title = title;
        this.Account_Name = Account_name;
        this.Description = description;
        this.Cost = cost;
        this.downpay = downpay;
        this.tradingRange = tradingRange;
        this.dept = dept;
        this.Cash_flow = cash_flow;
        this.Action = Action;
    }


    public float Downpay { get => downpay; set => downpay = value; }
    public string TradingRange { get => tradingRange; set => tradingRange = value; }
    public float Dept { get => dept; set => dept = value; }
    public float Cash_flow { get => cash_flow; set => cash_flow = value; }
}
