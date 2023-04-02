using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repay_Panel : MonoBehaviour
{
    [SerializeField] private List<GameObject> Pool_Game_Account_Template;

    public void Load_Game_Account_To_ListView(List<Game_accounts> accounts)
    {
        int count = 0;
        foreach(Game_accounts account in accounts)
        {
            if(account.Game_account_type == AccountType.Liability)
            {
                Pool_Game_Account_Template[count].gameObject.SetActive(true);
                Pool_Game_Account_Template[count].GetComponent<Dept_Template_Prefab>().Load_Game_Account(account);
                count++;
            }
        }
    }
    

    public void Clear()
    {
        foreach(GameObject obj in Pool_Game_Account_Template)
        {
            obj.gameObject.SetActive(false);
        }
    }
}
