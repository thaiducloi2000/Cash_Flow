using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NPCInteractable : MonoBehaviour
{
    [SerializeField] public string NPC_Name;
    [SerializeField] public TextMeshProUGUI Text;
    [SerializeField] public Button Button;
    [SerializeField] private PlayerInteract playerInteract;

    private void Update()
    {
        if(playerInteract.GetInteractableObject() != null)
        {
            Interact();
        }
        else
        {
            Uninteract();
        }
    }

    public void Uninteract()
    {
        Button.gameObject.SetActive(false);
        Text.gameObject.SetActive(false);
    }
    public void Interact()
    {
        Button.gameObject.SetActive(true);
        Text.gameObject.SetActive(true);
        Text.text = "Talk With " + NPC_Name + " To Play Game";
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("GamePlay_Scene");
    }
}
