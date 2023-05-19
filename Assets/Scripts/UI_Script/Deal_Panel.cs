using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System;

public class Deal_Panel : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI Title;
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private TextMeshProUGUI Cost_Txt;
    [SerializeField] private TextMeshProUGUI Mortgage_Divined_Txt;
    [SerializeField] private TextMeshProUGUI CashFlow_TradingRange_Txt;
    [SerializeField] private TextMeshProUGUI DownSize_Share_Owned_Txt;

    public void SetBigDeal(Big_Deal deal,Sprite image)
    {
        resetPanel();
        byte[] decodedBytes_Title = Encoding.UTF8.GetBytes(deal.Title);
        byte[] decodedBytes_Description = Encoding.UTF8.GetBytes(deal.Description);
        this._image.sprite = image;
        //this.Title.text = deal.Title;
        //this.Description.text = deal.Description;
        this.Title.text = Encoding.UTF8.GetString(decodedBytes_Title);
        this.Description.text = Encoding.UTF8.GetString(decodedBytes_Description);
        if (deal.Cost > 0)
        {
            this.Cost_Txt.text = "Chi Phí: " + deal.Cost + " $";
        }
        if (deal.Downpay > 0)
        {
            this.Mortgage_Divined_Txt.text = "Trả trước : " + deal.Downpay + " $";
        }
        if (deal.TradingRange != "0")
        {
            this.CashFlow_TradingRange_Txt.text = "Biến động giá: " + deal.TradingRange + " $";
        }if(deal.Dept > 0)
        {
            this.DownSize_Share_Owned_Txt.text = "Cổ phần sở hữu: " + deal.Dept + " $";
        }

    }

    public void SetSmallDeal(Small_Deal deal,Sprite image)
    {
        resetPanel();
        this._image.sprite = image;
        this.Title.text = deal.Title;
        this.Description.text = deal.Description;
        if (deal.Cost > 0)
        {
            this.Cost_Txt.text = "Cost : " + deal.Cost.ToString() + " $";
        }
        if(deal.Dept > 0)
        {
            this.Mortgage_Divined_Txt.text = "Dept : " + deal.Dept + " $";
        }
        if (deal.Trading_Range != "0")
        {
            this.CashFlow_TradingRange_Txt.text = "Trading Range : " + deal.Trading_Range + " $";
        }if (deal.Downsize > 0)
        {
            this.DownSize_Share_Owned_Txt.text = "Down Pay : " + deal.Downsize + " $";
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
