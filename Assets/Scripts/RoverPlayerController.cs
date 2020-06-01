using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoverPlayerController : MonoBehaviour
{
    // Transformation
    public Collider2D waterArea;
    public Collider2D landArea;
    public Text roverState;

    public GameObject ScanMessage; //added to display scan results
    public bool scanMode; //added to manage scan dialogue
    public bool socket; //added to check if socket has been activated
    private Text scanText;
    private RoverObject interactableTarget;

    // Appearance
    IsometricCharacterRenderer isoRenderer;

    // Movement
    private Rigidbody2D rbody;
    private float movementSpeed = 1f;

    // Battery
    public Slider batterySlider;
    public float timeLimitSeconds = 30f;
    public float transformCost = 3f; // In seconds.

    // Restart
    public GameObject restartButton;
    private Vector3 startPosition;
    private bool gameEndState = false;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();

        scanText = ScanMessage.GetComponentInChildren<Text>();
        scanText.text = ""; //line added for scanning purposes
        scanMode = false; //line added for scanning purposes
        ScanMessage.SetActive(scanMode);

        startPosition = GetComponent<Transform>().position;
        Restart();
    }


    // Update is called once per frame
    private void Update()
    {
        if (gameEndState) return;

        ScanCheck();

        TransformController();
        batterySlider.value -= Time.deltaTime / timeLimitSeconds;
    }

    void FixedUpdate()
    {
        if (gameEndState) return;
        if (scanMode) return;

        MovementController();
        if (batterySlider.value <= 0) GameEnd(false);
    }

    public void ChangeMovementSpeed(float newSpeed) { movementSpeed = newSpeed; }

    public void SetInteractableTarget(RoverObject target) { interactableTarget = target; }

    private void ScanCheck()
    {
        
        // Scan key is pressed AND rover is close to some interactable object
        if (Input.GetKeyDown("h") && interactableTarget != null)
        {
            Debug.Log("GetKeyDown is h");
            if (scanMode == false) 
            {
                scanMode = true;
                scanText.text = interactableTarget.scanMessage;
            }
            else
            {
                Debug.Log("Now disabling scanMode");
                scanMode = false;
                scanText.text = "";
            }
            ScanMessage.SetActive(scanMode);
        }
    }
    
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
        batterySlider.value -= transformCost / timeLimitSeconds;
        gameObject.tag = goal;
        roverState.text = goal;
        Debug.Log("Transforming to: " + goal);
    }

    private void GameEnd(bool success)
    {
        gameEndState = true;
        if (success) {
            scanText.text = "You feel the soul of your own kind within this alien object.";
        } else {
            scanText.text = "You feel the last trickle of power leaving the system.";
        }
        ScanMessage.SetActive(true);
        restartButton.SetActive(true);
    }

    public void Restart()
    {
        GetComponent<Transform>().position = startPosition;
        batterySlider.value = 1.0f; // Battery start full.
        restartButton.SetActive(false);
        gameEndState = false;
        ScanMessage.SetActive(false);
    }
}
