using Fusion;
using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Doodad_Panel : NetworkBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private TextMeshProUGUI Cost_Txt;
    public Doodad doodad;

    public void SetDoodadPanel(Doodad doodad, Sprite image)
    {
        this._image.sprite = image;
        this.doodad = doodad;

        byte[] decodedBytes_Description = Encoding.UTF8.GetBytes(doodad.Description);
        this.Description.text = Encoding.UTF8.GetString(decodedBytes_Description);
        this.Cost_Txt.text = "Chi Phí: " + doodad.Cost + " $";

    }

    public void AcceptDoodad()
    {
        if (Player.Instance.financial_rp.GetCash() >= this.doodad.Cost)
        {
            this.gameObject.SetActive(false);
            UI_Manager.instance.UpdateProfilePlayer();
            Player.Instance.financial_rp.SetCash(Player.Instance.financial_rp.GetCash() - doodad.Cost);
        }
        else
        {
            Rental_Panel rent_panel = UI_Manager.instance.Rental_Panel.GetComponent<Rental_Panel>();
            rent_panel.Show_Penel();
        }
    }
}
