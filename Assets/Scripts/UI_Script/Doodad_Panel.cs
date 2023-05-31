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

    public void SetDoodadPanel(Doodad doodad/*, Sprite image*/)
    {
        //this._image.sprite = image;
        this.doodad = doodad;

        byte[] decodedBytes_Description = Encoding.UTF8.GetBytes(doodad.Description);
        //this.Description.text = doodad.Description;
        this.Description.text = Encoding.UTF8.GetString(decodedBytes_Description);
        this.Cost_Txt.text = "Chi Phí: " + doodad.Cost + " $";

    }

    public void AcceptDoodad()
    {
        //Player player = GetComponentInChildren<Player>();
        this.gameObject.SetActive(false);
        //Player.Instance.GetComponent<Player>().UpdatePlayerTurn();
        UI_Manager.instance.UpdateProfilePlayer();
        //GameManager.Instance.RPC_nextTurn();
    }
}
