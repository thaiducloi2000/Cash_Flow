using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dept_Template_Prefab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI account_name;
    [SerializeField] private TextMeshProUGUI money;
    private Game_accounts account;

    public void Load_Game_Account(Game_accounts account)
    {
        this.account = account;
        this.account_name.text = account.Game_account_name;
        this.money.text = account.Game_account_value.ToString();
    }

    public void Repay()
    {
        if(Player.Instance.financial_rp.GetCash() < this.account.Game_account_value)
        {
            Debug.Log(this.account.Game_account_value);
        }
        else
        {
            Player.Instance.financial_rp.SetCash(Player.Instance.financial_rp.GetCash() - this.account.Game_account_value);
            Player.Instance.financial_rp.game_accounts.Remove(this.account);
            List<Game_accounts> list = new List<Game_accounts>(Player.Instance.financial_rp.game_accounts);
            foreach (Game_accounts account in list)
            {
                if (Find_Expense(account.Game_account_name, this.account.Game_account_name) && account.Game_account_type == AccountType.Expense)
                {
                    Player.Instance.financial_rp.game_accounts.Remove(account);
                }
            }
            this.gameObject.GetComponentInParent<Repay_Panel>().Load_Game_Account_To_ListView();
            this.gameObject.SetActive(false);
        }
    }

    public bool Find_Expense(string account1,string account2)
    {
        string[] words1 = account1.Split(' ');
        string[] words2 = account2.Split(' ');

        foreach(string word1 in words1)
        {
            foreach(string word2 in words2)
            {
                if (word1.Equals(word2))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
