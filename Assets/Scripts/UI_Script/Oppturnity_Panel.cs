using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Oppturnity_Panel : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI Title;
    [SerializeField] private TextMeshProUGUI Description;

    public void SetBigDeal(Big_Deal deal, Sprite image)
    {
        resetPanel();
        this._image.sprite = image;
        byte[] decodedBytes_Title = Encoding.UTF8.GetBytes(deal.Title);
        byte[] decodedBytes_Description = Encoding.UTF8.GetBytes(deal.Description);
        this.Title.text = Encoding.UTF8.GetString(decodedBytes_Title);
        this.Description.text = "Trả trước: " + deal.Downpay + "<space=5em>Dòng tiền: " + deal.Cash_flow;
    }

    private void resetPanel()
    {
        this.Title.text = "";
        this.Description.text = "";
    }
}
