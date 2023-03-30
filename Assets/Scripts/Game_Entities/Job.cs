using System.Collections.Generic;


public class Job
{
    public string id { get; set; }
    public string Job_card_name { get; set; }
    public float Children_cost { get; set; }
    public List<Game_accounts> Game_accounts { get; set; }

    public Job(string _id, string name, float children_cost, List<Game_accounts> gameAccounts)
    {
        this.Game_accounts = gameAccounts;
        this.id = _id;
        this.Job_card_name = name;
        this.Children_cost = children_cost;
    }

}
