using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeName : MonoBehaviour
{
    public GameObject ChangeNamePanel;
    public TMP_InputField EditName_textfield;
    public TMP_Text infoname;

    public void closePanel()
    {
        ChangeNamePanel.SetActive(false);
    }
    public void OpenPanel()
    {
        ChangeNamePanel.SetActive(true);
        EditName_textfield.text = infoname.text;
    }

    public void Editname()
    {
        infoname.text = EditName_textfield.text;
        ChangeNamePanel.SetActive(false);
    }
}
