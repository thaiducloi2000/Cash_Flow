using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Doodad_Panel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private TextMeshProUGUI Cost_Txt;
    public Doodad doodad;

    public void SetDoodadPanel(Doodad doodad)
    {
        this.doodad = doodad;
        this.Description.text = doodad.Description;
        this.Cost_Txt.text = "Cost : " + doodad.Cost;

    }

    public void AcceptDoodad()
    {
        //Player player = GetComponentInChildren<Player>();
        this.gameObject.SetActive(false);
    }
}
