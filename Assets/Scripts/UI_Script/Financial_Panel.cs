using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Financial_Panel : MonoBehaviour
{
    [SerializeField] private GameObject Job_Name;
    [SerializeField] private GameObject Content_Incomes;
    [SerializeField] private GameObject Content_Assets;
    [SerializeField] private GameObject Content_Expenses;
    [SerializeField] private GameObject Content_Liabilities;
    [SerializeField] private TextMeshProUGUI Total_Income;
    [SerializeField] private TextMeshProUGUI Total_Expense;
    [SerializeField] private TextMeshProUGUI Income_Goal;
    [SerializeField] private Slider Goal_Percent;
    public void loadFinInformation(Financial fin)
    {
        resetText();
        float total_income = 0;
        float total_asset = 0;
        float total_expense = 0;
        float total_liability = 0;
        float salary = 0;
        Job_Name.GetComponent<TextMeshProUGUI>().text = fin.job_card_id;
        foreach (Game_accounts account in fin.game_accounts)
        {
            switch (account.Game_account_type)
            {
                case AccountType.Income:
                    if (account.Game_account_name == "Salary")
                    {
                        Content_Incomes.GetComponent<ScrollRect>().content.GetComponentInChildren<TextMeshProUGUI>().text += account.Game_account_name + ": $ " + account.Game_account_value + '\n';
                        salary += account.Game_account_value;
                    }
                    else
                    {
                        Content_Incomes.GetComponent<ScrollRect>().content.GetComponentInChildren<TextMeshProUGUI>().text += account.Amount + " " + account.Game_account_name + ": $ " + account.Game_account_value + '\n';
                    }
                    total_income += account.Game_account_value;
                    break;
                case AccountType.Asset:
                    if (account.Game_account_name == "cash")
                    {
                        Content_Assets.GetComponent<ScrollRect>().content.GetComponentInChildren<TextMeshProUGUI>().text += account.Game_account_name + ": $ " + account.Game_account_value + '\n';
                    }
                    else
                    {
                        Content_Assets.GetComponent<ScrollRect>().content.GetComponentInChildren<TextMeshProUGUI>().text += account.Amount + " " + account.Game_account_name + ": $" + account.Game_account_value + '\n';
                    }
                    total_asset += account.Game_account_value;
                    break;
                case AccountType.Expense:
                    Content_Expenses.GetComponent<ScrollRect>().content.GetComponentInChildren<TextMeshProUGUI>().text += account.Game_account_name + ": $ " + account.Game_account_value + '\n';
                    total_expense += account.Game_account_value;
                    break;
                case AccountType.Liability:
                    Content_Liabilities.GetComponent<ScrollRect>().content.GetComponentInChildren<TextMeshProUGUI>().text += account.Game_account_name + ": $ " + account.Game_account_value + '\n';
                    total_liability += account.Game_account_value;
                    break;
                default:
                    break;
            }
        }
        Total_Income.text = "Total Income : " + total_income + "$";
        Debug.Log(fin.GetCash());
        Total_Expense.text = "Total Expense : " + total_expense + "$";
        Goal_Percent.value = ((total_income - salary) / total_expense);
        Income_Goal.text = Mathf.Round((total_income - salary) / total_expense * 100f) + "%";
    }

    private void resetText()
    {
        Content_Incomes.GetComponent<ScrollRect>().content.GetComponentInChildren<TextMeshProUGUI>().text = "";
        Content_Assets.GetComponent<ScrollRect>().content.GetComponentInChildren<TextMeshProUGUI>().text = "";
        Content_Expenses.GetComponent<ScrollRect>().content.GetComponentInChildren<TextMeshProUGUI>().text = "";
        Content_Liabilities.GetComponent<ScrollRect>().content.GetComponentInChildren<TextMeshProUGUI>().text = "";
        Total_Income.text = "";
        Income_Goal.text = "";
        Total_Expense.text = "";

    }

}
