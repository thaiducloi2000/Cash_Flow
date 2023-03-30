using UnityEngine;
using TMPro;


public class Deal_Panel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Title;
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private TextMeshProUGUI Cost_Txt;
    [SerializeField] private TextMeshProUGUI Mortgage_Divined_Txt;
    [SerializeField] private TextMeshProUGUI CashFlow_TradingRange_Txt;
    [SerializeField] private TextMeshProUGUI DownSize_Share_Owned_Txt;


    public void SetBigDeal(Big_Deal deal)
    {
        resetPanel();
        this.Title.text = deal.Title;
        this.Description.text = deal.Description;
        if (deal.Cost > 0)
        {
            this.Cost_Txt.text = "Cost : " + deal.Cost;
        }
        if (deal.Downpay > 0)
        {
            this.Mortgage_Divined_Txt.text = "Down pay : " + deal.Downpay;
        }
        if (deal.TradingRange != "0")
        {
            this.CashFlow_TradingRange_Txt.text = "Trading Range : " + deal.TradingRange;
        }if(deal.Dept > 0)
        {
            this.DownSize_Share_Owned_Txt.text = "Share Owned : " + deal.Dept;
        }

    }

    public void SetSmallDeal(Small_Deal deal)
    {
        resetPanel();
        this.Title.text = deal.Title;
        this.Description.text = deal.Description;
        if (deal.Cost > 0)
        {
            this.Cost_Txt.text = "Cost : " + deal.Cost.ToString();
        }
        if(deal.Dept > 0)
        {
            this.Mortgage_Divined_Txt.text = "Dept : " + deal.Dept;
        }
        if (deal.Trading_Range != "0")
        {
            this.CashFlow_TradingRange_Txt.text = "Trading Range : " + deal.Trading_Range;
        }if (deal.Downsize > 0)
        {
            this.DownSize_Share_Owned_Txt.text = "Down Pay : " + deal.Downsize;
        }
    }
    

    private void resetPanel()
    {
        this.Title.text = "";
        this.Description.text = "";
        this.Cost_Txt.text = "";
        this.Mortgage_Divined_Txt.text = "";
        this.CashFlow_TradingRange_Txt.text = "";
        this.DownSize_Share_Owned_Txt.text = "";
    }
}
