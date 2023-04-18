using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon_Spinner : MonoBehaviour
{
    private float angle;
    // Update is called once per frame
    void Update()
    {
        Spint();
    }

    void Spint()
    {
        angle += -Time.deltaTime * 200f;
        this.transform.rotation = Quaternion.Euler(0,0,angle);
    }
}
