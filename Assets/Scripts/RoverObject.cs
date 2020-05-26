using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverObject : MonoBehaviour
{
    public bool isLifeInteractable = true;
    public string scanMessage = "It doesn't seem to be life.";

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        RoverPlayerController controller = collision.gameObject.GetComponent<RoverPlayerController>();
        if (!controller) return;
        controller.SetInteractableTarget(this);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        
        RoverPlayerController controller = collision.gameObject.GetComponent<RoverPlayerController>();
        if (!controller) return;
        controller.SetInteractableTarget(null);
    }
}