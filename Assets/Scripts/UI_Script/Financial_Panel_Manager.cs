using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Financial_Panel_Manager : MonoBehaviour
{
    [SerializeField] private GameObject financial;
    [SerializeField] private GameObject financial_fat_race;
    public bool isOpen = false;

    [SerializeField] private Financial financial_rp_rat_race;
    [SerializeField] private Financial financial_rp_fat_race;





    public void viewFinancial()
    {
        isOpen = !isOpen;
        if (Player.Instance.isInFatRace)
        {
            this.financial_fat_race.SetActive(isOpen);
            //this.financial_fat_race.GetComponent<Financial_Panel>().loadFinInformation(this.financial_rp_fat_race);
        }
        else
        {
            this.financial.SetActive(isOpen);
            this.financial.GetComponent<Financial_Panel>().loadFinInformation(this.financial_rp_rat_race);
        }
    }

    public void Financial(Financial fin)
    {
        if(Player.Instance.isInFatRace)
        {
            this.financial_rp_fat_race = fin;
        }
        else
        {
            this.financial_rp_rat_race = fin;
        }
    }
}
