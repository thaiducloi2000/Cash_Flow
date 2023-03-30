using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMGMusic : MonoBehaviour
{
    public static BMGMusic instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
