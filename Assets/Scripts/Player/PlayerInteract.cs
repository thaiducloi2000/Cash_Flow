using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
    }

    //public void CheckInRange()
    //{
    //    float interactRange = 2f;
    //    Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
    //    foreach(Collider collider in colliderArray)
    //    {
    //        if(collider.TryGetComponent(out NPCInteractable npcInteractable))
    //        {
    //            npcInteractable.Interact();
    //        }
    //    }
    //}

    public NPCInteractable GetInteractableObject()
    {
        float interactRange = 1f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out NPCInteractable npcInteractable))
            {
                return npcInteractable;
            }
        }
        return null;
    }
}
