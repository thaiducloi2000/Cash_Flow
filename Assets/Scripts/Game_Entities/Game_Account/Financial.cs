
using System.Collections.Generic;

public class Financial
{
    public int children_amount { get; set; }
    public string user_id { get; set; }
    public string job_card_id { get; set; }
    public float income_per_month { get; set; }
    public float expense_per_month { get; set; }
    public List<Game_accounts> game_accounts { get; set; }

    public Financial(int children_amount, string user_id, string job_card_id, float income_per_month, float expense_per_month, List<Game_accounts> game_accounts)
    {
        this.children_amount = children_amount;
        this.user_id = user_id;
        this.job_card_id = job_card_id;
        this.income_per_month = income_per_month;
        this.expense_per_month = expense_per_month;
        this.game_accounts = game_accounts;
    }

    public float GetCash()
    {
        float cash = 0;
        foreach (Game_accounts accounts in game_accounts)
        {
            if (accounts.Game_account_type == AccountType.Asset && accounts.Game_account_name == "cash")
            {
                cash = accounts.Game_account_value;
            }
        }
        return cash;
    }
    public void SetCash(float cash)
    {
        foreach (Game_accounts accounts in game_accounts)
        {
            if (accounts.Game_account_type == AccountType.Asset && accounts.Game_account_name == "cash")
            {
                accounts.Game_account_value = cash;
            }
        }
    }

    public float GetPassiveIncome()
    {
        float passiveIncome = 0;
        foreach(Game_accounts account in game_accounts)
        {
            if(account.Game_account_type == AccountType.Expense)
            {
                passiveIncome -= account.Game_account_value;
            }
            if(account.Game_account_type == AccountType.Income && account.Game_account_name != "Salary")
            {
                passiveIncome += account.Game_account_value;
            }
        }
        return passiveIncome;
    }

}
