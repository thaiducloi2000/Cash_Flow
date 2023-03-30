
public class Expense : Game_accounts
{
    public Expense(string gameaccount_name, float gameaccount_money,int amount)
    {
        this.Game_account_type = AccountType.Expense;
        this.Game_account_name = gameaccount_name;
        this.Game_account_value = gameaccount_money;
        this.Amount = amount;
    }

    public override float Account_Balance()
    {
        return -base.Account_Balance();
    }
}
