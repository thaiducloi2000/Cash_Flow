using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Repay_Panel : MonoBehaviour
{
    [SerializeField] private List<GameObject> Pool_Game_Account_Template;
    [SerializeField] private Dept_Template_Prefab Dept_Template;
    [SerializeField] private TextMeshProUGUI Balance;
    [SerializeField] private GameObject Content;


    private void Awake()
    {
        this.Pool_Game_Account_Template = new List<GameObject>();
        for(int i = 0; i < 7; i++)
        {
            GameObject account = Instantiate(Dept_Template.gameObject, Content.transform);
            Pool_Game_Account_Template.Add(account);
            account.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void Load_Game_Account_To_ListView()
    {
        int count = 0;
        foreach(Game_accounts account in Player.Instance.financial_rp.game_accounts)
        {
            if(account.Game_account_type == AccountType.Liability)
            {
                Pool_Game_Account_Template[count].gameObject.SetActive(true);
                Pool_Game_Account_Template[count].GetComponent<Dept_Template_Prefab>().Load_Game_Account(account);
                count++;
            }
        }
        Balance.text = Player.Instance.financial_rp.GetCash().ToString();
    }
    

    public void Clear()
    {
        foreach(GameObject obj in Pool_Game_Account_Template)
        {
            obj.gameObject.SetActive(false);
        }
    }
}
