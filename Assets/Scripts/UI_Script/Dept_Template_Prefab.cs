using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dept_Template_Prefab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI account_name;
    [SerializeField] private TextMeshProUGUI money;

    public void Load_Game_Account(Game_accounts account)
    {
        this.account_name.text = account.Game_account_name;
        this.money.text = account.Game_account_value.ToString();
    }
}
