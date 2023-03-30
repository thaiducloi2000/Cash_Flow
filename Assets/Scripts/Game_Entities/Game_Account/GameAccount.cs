
public enum AccountType { Income,Expense,Asset,Liability}
public class Game_accounts
{


    public AccountType Game_account_type { get; set; }
    public string Game_account_name { get; set; }
    public float Game_account_value { get; set; }

    public int Amount { get; set; }



    public virtual float Account_Balance()
    {
        return this.Game_account_value;
    }

}
