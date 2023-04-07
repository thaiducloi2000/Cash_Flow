using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Doodad_Panel : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private TextMeshProUGUI Cost_Txt;
    public Doodad doodad;

    public void SetDoodadPanel(Doodad doodad,Sprite image)
    {
        this._image.sprite = image;
        this.doodad = doodad;
        this.Description.text = doodad.Description;
        this.Cost_Txt.text = "Cost : " + doodad.Cost + " $";

    }

    public void AcceptDoodad()
    {
        //Player player = GetComponentInChildren<Player>();
        this.gameObject.SetActive(false);
    }
}
