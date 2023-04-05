using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Financial_Fat_Panel : MonoBehaviour
{
    [SerializeField] private GameObject Content_Incomes;
    [SerializeField] private TextMeshProUGUI Cash_Flow_Day;
    [SerializeField] private TextMeshProUGUI Cash;
    [SerializeField] private TextMeshProUGUI Income;
    [SerializeField] private Slider Goal_Percent;
    public void loadFinInformation(Financial fin)
    {
        resetText();
        float total_income = 0;
        float total_expense = 0;
        float salary = 0;
        foreach (Game_accounts account in fin.game_accounts)
        {
            //if()
            Content_Incomes.GetComponent<ScrollRect>().content.GetComponentInChildren<TextMeshProUGUI>().text += account.Game_account_name + ": $ " + account.Game_account_value + '\n';
            salary += account.Game_account_value;
        }
        Cash_Flow_Day.text = total_income.ToString();
        Cash.text = fin.GetCash().ToString();
        Goal_Percent.value = ((total_income - salary) / 50000);
        Income.text = (total_income - salary).ToString();
    }

    private void resetText()
    {
        Content_Incomes.GetComponent<ScrollRect>().content.GetComponentInChildren<TextMeshProUGUI>().text = "";
        Cash_Flow_Day.text = "";
        Income.text = "";
        Cash.text = "";

    }
}
