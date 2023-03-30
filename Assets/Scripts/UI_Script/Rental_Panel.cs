using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class Rental_Panel : MonoBehaviour
{
    [SerializeField] private TMP_InputField money;
    [SerializeField] public GameObject Loan_Panel;
    public bool showPenel = false;
    private const float default_rent_money = 1000;
    private float tmp_rent_money;


    private void Start()
    {
        showPenel = false;
    }
    public void Show_Penel()
    {
        showPenel = !showPenel;
        Reset_Rent_Value();
        this.Loan_Panel.SetActive(showPenel);
    }

    public void Up_Rent_Value()
    {
        tmp_rent_money += default_rent_money;
        money.text = tmp_rent_money.ToString();
    }


    public void Down_Rent_Value()
    {
        if (tmp_rent_money > 0)
        {
            tmp_rent_money -= default_rent_money;
        }
        money.text = tmp_rent_money.ToString();
    }
    private void Reset_Rent_Value()
    {
        tmp_rent_money = default_rent_money;
        money.text = tmp_rent_money.ToString();
    }

    public void Accept_Loan()
    {
        float cash = float.Parse(money.text.Trim());
        if (HasLoan())
        {
            Update_Financial(cash);
        }
        else
        {
            Liability lia = new Liability("Loan", cash, 1);
            Expense expense = new Expense("Loan Expense", lia.Game_account_value * 0.01f, 1);
            Player.Instance.financial_rp.SetCash(cash + Player.Instance.financial_rp.GetCash());
            Player.Instance.financial_rp.game_accounts.Add(lia);
            Player.Instance.financial_rp.game_accounts.Add(expense);
        }
        Show_Penel();
    }

    private bool HasLoan()
    {
        foreach (Game_accounts account in Player.Instance.financial_rp.game_accounts)
        {
            if (account.Game_account_name == "Loan" && account.Game_account_type == AccountType.Liability)
            {
                return true;
            }
        }
        return false;
    }

    private void Update_Financial(float cash)
    {
        float tmp = 0;

        foreach (Game_accounts account in Player.Instance.financial_rp.game_accounts)
        {
            if (account.Game_account_name == "Loan" && account.Game_account_type == AccountType.Liability)
            {
                account.Game_account_value += cash;
                tmp = account.Game_account_value;
            }
        }
        foreach (Game_accounts account in Player.Instance.financial_rp.game_accounts)
        {
            if (account.Game_account_name == "Loan Expense" && account.Game_account_type == AccountType.Expense)
            {
                account.Game_account_value = tmp * 0.01f;
            }
        }
        Player.Instance.financial_rp.SetCash(cash + Player.Instance.financial_rp.GetCash());
    }
}
