using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProblemContainer_Panel : MonoBehaviour
{

    public GameObject Divorce_Panel;
    public GameObject Litigation_Panel;
    public GameObject Taxes_Panel;
    public GameObject BrokenCar_Panel;

    private void Start()
    {
        Reset();
    }

    public void Popup_Divorce_Panel()
    {
        Divorce_Panel.gameObject.SetActive(true);
    }

    public void Popup_BrokenCar_Panel()
    {
        BrokenCar_Panel.gameObject.SetActive(true);
    }

    public void Popup_Litigation_Panel()
    {
        Litigation_Panel.gameObject.SetActive(true);
    }

    public void Popup_Taxes_Panel()
    {
        Taxes_Panel.gameObject.SetActive(true);
    }

    public void Drop_Panel()
    {
        Reset();
    }

    public void Reset()
    {
        Divorce_Panel.SetActive(false);
        Litigation_Panel.SetActive(false);
        Taxes_Panel.SetActive(false);
        BrokenCar_Panel.SetActive(false);
    }
}
