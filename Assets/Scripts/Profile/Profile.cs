using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Profile : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ProfilePanel;
    public TMP_Text profilename;
    public TMP_Text infoname;
    private void Update()
    {
        profilename.text = infoname.text;
    }
    public void openProfile()
    {
        ProfilePanel.SetActive(true);
        profilename.text = infoname.text;
    }
    public void closeProfile()
    {
        ProfilePanel.SetActive(false);
    }
}
