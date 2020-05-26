using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverPlayerController : MonoBehaviour
{
    public Collider2D waterArea;
    public Collider2D landArea;
    
    IsometricCharacterRenderer isoRenderer;

    private Rigidbody2D rbody;
    private float movementSpeed = 1f;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        TransformController();
        MovementController();
       
    }

    public void ChangeMovementSpeed(float newSpeed) { movementSpeed = newSpeed; }

    private void MovementController()
    {
        // Handle Movement
        Vector2 currentPos = rbody.position;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        Vector2 movement = inputVector * movementSpeed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        isoRenderer.SetDirection(movement);
        rbody.MovePosition(newPos);
    }

    private void TransformController()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            TransformMode("SwimRover");
            waterArea.isTrigger = true;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            TransformMode("LandRover");
            waterArea.isTrigger = false;
        }
    }

    private void TransformMode(string goal)
    {
        gameObject.tag = goal;
        Debug.Log("Transforming to: " + goal);
    }
}
