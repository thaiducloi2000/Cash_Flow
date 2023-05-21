
using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Market_Panel : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI Title;
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private TextMeshProUGUI Cost_Txt;
    public Market market;


    public void SetMarketPanel(Market market/*,Sprite image*/)
    {
        //this._image.sprite = image;
        this.market = market;
        byte[] decodedBytes_Title = Encoding.UTF8.GetBytes(market.Title);
        byte[] decodedBytes_Description = Encoding.UTF8.GetBytes(market.Description);
        //this.Title.text = market.Title;
        //this.Description.text = market.Description;
        this.Title.text = Encoding.UTF8.GetString(decodedBytes_Title);
        this.Description.text = Encoding.UTF8.GetString(decodedBytes_Description);
        if (market.Cost > 0)
        {
            this.Cost_Txt.text = "Chi Phí: " + market.Cost + " $/ 1 Unit";
        }

    }

    public void AcceptMarket()
    {
        foreach(Game_accounts account in Player.Instance.financial_rp.game_accounts)
        {
            if(account.Game_account_name == market.Account_Name)
            {
                switch (market.Action)
                {
                    case 2:
                        Player.Instance.financial_rp.game_accounts.Remove(account);
                        Player.Instance.financial_rp.SetCash(Player.Instance.financial_rp.GetCash() + market.Cost);
                        break;
                    case 4:
                        account.Game_account_value *= 2;
                        break;
                    case 7:
                        if (market.Cost == 0)
                        {
                            account.Game_account_value *= 2;
                        }
                        else
                        {
                            account.Game_account_value = market.Cost;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        this.gameObject.SetActive(false);
        //Player.Instance.GetComponent<Player>().UpdatePlayerTurn();
        UI_Manager.instance.UpdateProfilePlayer(); 
        //GameManager.Instance.RPC_nextTurn();
    }
}
