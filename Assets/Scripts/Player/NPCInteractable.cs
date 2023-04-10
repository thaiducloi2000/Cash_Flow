using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCInteractable : MonoBehaviour
{
    public void Interact()
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("GamePlay_Scene");
    }
}
