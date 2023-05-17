using System.Collections;
using System.Collections.Generic;
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
        this.Title.text = deal.Title;
        this.Description.text = deal.Description + "<br> Trả trước: "+deal.Downpay + "<space=5em>Dòng tiền: "+deal.Cash_flow;
    }

    private void resetPanel()
    {
        this.Title.text = "";
        this.Description.text = "";
    }
}
