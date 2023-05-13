using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class History : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Historypanel;

    public void open()
    {
        Historypanel.SetActive(true);
    }
    public void close()
    {
        Historypanel.SetActive(false);
    }
}
