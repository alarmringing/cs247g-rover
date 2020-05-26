using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverColliderManager : MonoBehaviour
{
    public enum ColliderType { land, water };


    public ColliderType colliderType;

    private Collider2D collider;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        RoverPlayerController controller = collision.gameObject.GetComponent<RoverPlayerController>();
        if (!controller) return;

        switch(colliderType)
        {
            case ColliderType.land:
                if (controller.tag == "SwimRover") controller.ChangeMovementSpeed(0.3f);
                else controller.ChangeMovementSpeed(1f);
                return;
            case ColliderType.water:
                if (controller.tag == "SwimRover") controller.ChangeMovementSpeed(1f);
                return;
        }
    }

}
