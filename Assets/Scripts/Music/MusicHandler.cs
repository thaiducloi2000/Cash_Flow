using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicHandler : MonoBehaviour
{
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "GamePlay_Scene")
        {
            BMGMusic.instance.GetComponent<AudioSource>().Pause();
        }
    }
}
