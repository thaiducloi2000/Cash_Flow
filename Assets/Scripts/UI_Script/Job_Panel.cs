using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Job_Panel : MonoBehaviour
{
    [SerializeField] private GameObject Job_Name;
    [SerializeField] private GameObject Content_Incomes;
    [SerializeField] private GameObject Content_Assets;
    [SerializeField] private GameObject Content_Expenses;
    [SerializeField] private GameObject Content_Liabilities;
    [SerializeField] private TextMeshProUGUI total_Income;
    [SerializeField] private TextMeshProUGUI total_Asset;
    [SerializeField] private TextMeshProUGUI total_Expense;
    [SerializeField] private TextMeshProUGUI total_Liabilities;
    public float total_income = 0;
    public float total_asset = 0;
    public float total_expense = 0;
    public float total_liability = 0;
    public Job job;

    public void loadJobInformation(Job job)
    {
       
        Job_Name.GetComponent<TextMeshProUGUI>().text = job.Job_card_name;
        foreach(Game_accounts account in job.Game_accounts)
        {
            switch (account.Game_account_type)
            {
                case AccountType.Income:
                    Content_Incomes.GetComponent<ScrollRect>().content.GetComponentInChildren<TextMeshProUGUI>().text += account.Game_account_name + ": $" + account.Game_account_value + '\n';
                    total_income += account.Game_account_value;
                    break;
                case AccountType.Asset:
                    Content_Assets.GetComponent<ScrollRect>().content.GetComponentInChildren<TextMeshProUGUI>().text += account.Game_account_name + ": $" + account.Game_account_value + '\n';
                    total_asset += account.Game_account_value;
                    break;
                case AccountType.Expense:
                    Content_Expenses.GetComponent<ScrollRect>().content.GetComponentInChildren<TextMeshProUGUI>().text += account.Game_account_name + ": $" + account.Game_account_value + '\n';
                    total_expense += account.Game_account_value;
                    break;
                case AccountType.Liability:
                    Content_Liabilities.GetComponent<ScrollRect>().content.GetComponentInChildren<TextMeshProUGUI>().text += account.Game_account_name + ": $" + account.Game_account_value + '\n';
                    total_liability += account.Game_account_value;
                    break;
                default:
                    break;
            }
        }
        this.job = job;
        total_Income.text = "Tổng Thu nhập: " + total_income;
        total_Asset.text = "Tổng tài sản: " + total_asset;
        total_Expense.text = "Tổng chi phí: " + total_expense;
        total_Liabilities.text = "Tổng Nợ: " + total_liability;
    }
}
