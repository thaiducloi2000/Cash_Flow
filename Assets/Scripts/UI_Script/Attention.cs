using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Attention : MonoBehaviour
{
    public GameObject Attentionpanel;
    public GameObject AttentionSuccesspanel;
    public GameObject Loginpanel;
    public GameObject Regiserpanel;
    // Start is called before the first frame update
    public void ok()
    {
        Attentionpanel.SetActive(false);
    }
    public void oksuccess()
    {
        Attentionpanel.SetActive(false);
        Loginpanel.SetActive(true);
        Regiserpanel.SetActive(false);
    }
}
