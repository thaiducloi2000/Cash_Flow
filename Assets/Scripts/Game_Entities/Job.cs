using System;
using System.Collections.Generic;
using UnityEngine;


public class Job
{
    public string id { get; set; }
    public string Job_card_name { get; set; }
    public int Children_cost { get; set; }
    public string Image_url { get; set; }
    public bool Status { get; set; }
    public List<Game_accounts> Game_accounts { get; set; }


    public Job(string _id, string name, int children_cost, List<Game_accounts> gameAccounts)
    {
        this.Game_accounts = gameAccounts;
        this.id = _id;
        this.Job_card_name = name;
        this.Children_cost = children_cost;
    }

    public float GetSalary()
    {
        float cash_value = 0;
        foreach(Game_accounts account in Game_accounts)
        {
            if(account.Game_account_name == "Salary")
            {
                cash_value = account.Game_account_value;
            }
        }
        return cash_value;
    }

    public float GetCash()
    {
        float cash_value = 0;
        foreach (Game_accounts account in Game_accounts)
        {
            if (account.Game_account_name == "cash")
            {
                cash_value = account.Game_account_value;
            }
        }
        return cash_value;
    }
    public float GetTax()
    {
        float cash_value = 0;
        foreach (Game_accounts account in Game_accounts)
        {
            if (account.Game_account_name == "Tax")
            {
                cash_value = account.Game_account_value;
            }
        }
        return cash_value;
    }
}
