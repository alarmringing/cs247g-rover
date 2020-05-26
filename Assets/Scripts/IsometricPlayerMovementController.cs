using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //to make use of Text

public class IsometricPlayerMovementController : MonoBehaviour
{

    public float movementSpeed = 1f;
    IsometricCharacterRenderer isoRenderer;

    Rigidbody2D rbody;

    public Text scanText; //added to display scan results
    public Image scanTextBox; //added to display scan results
    public bool scanMode; //added to manage scan dialogue
    public bool socket; //added to check if socket has been activated

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();
        scanMode = false; //line added for scanning purposes
        scanText.text = ""; //line added for scanning purposes
        scanText.fontSize = 24;
        scanTextBox = GetComponentInChildren<Image>(); //a whole lot of code just to change textbox alpha, smh
        Color c = scanTextBox.color;
        c.a = 0;
        scanTextBox.color = c;
        socket = false;
    }

    void Update()
    {
        Vector2 barrelPos = new Vector2(-4.4f, -0.65f);
        Vector2 computerPos = new Vector2(3.15f, 4.00f);
        Vector2 socketPos = new Vector2(-7.0f, 3.8f);
        Vector2 treePos = new Vector2(-2.0f, 1.3f);
        if(Input.GetKeyDown("h")){
            Vector2 currentPos = rbody.position;
            Debug.Log(currentPos);
            //Debug.Log(Vector2.Distance(currentPos, barrelPos));
            if(scanMode == false){
                if(Vector2.Distance(currentPos, barrelPos) < 0.5f){
                    //Debug.Log("Yes, that is a fucking barrel");
                    scanText.text = "Yes, that is a f*cking barrel";
                    scanTextBox = GetComponentInChildren<Image>();
                    Color c = scanTextBox.color;
                    c.a = 0.8f;
                    scanTextBox.color = c;
                    scanMode = true;
                }else if(Vector2.Distance(currentPos, treePos) < 0.5f){
                    scanText.text = "Why this tree? I dunno either";
                    scanTextBox = GetComponentInChildren<Image>();
                    Color c = scanTextBox.color;
                    c.a = 0.8f;
                    scanTextBox.color = c;
                    scanMode = true;
                }else if(Vector2.Distance(currentPos, computerPos) < 0.5f){
                    if (socket){
                        scanText.text = "You've found life, go Team You";
                        scanTextBox = GetComponentInChildren<Image>();
                        Color c = scanTextBox.color;
                        c.a = 0.8f;
                        scanTextBox.color = c;
                        scanMode = true;
                    }else{
                        scanText.text = "Out for lunch, check back later";
                        scanTextBox = GetComponentInChildren<Image>();
                        Color c = scanTextBox.color;
                        c.a = 0.8f;
                        scanTextBox.color = c;
                        scanMode = true;
                    }
                }else if(Vector2.Distance(currentPos, socketPos) < 0.5f){
                    scanText.text = "You feel a tingle, and not the good kind";
                    scanTextBox = GetComponentInChildren<Image>();
                    Color c = scanTextBox.color;
                    c.a = 0.8f;
                    scanTextBox.color = c;
                    scanMode = true;
                    socket = true;
                }
            }else{
                scanMode = false;
                scanText.text = "";
                scanTextBox = GetComponentInChildren<Image>();
                Color c = scanTextBox.color;
                c.a = 0;
                scanTextBox.color = c;
            }
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (scanMode == false){ //added to ensure no movement while scanning
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
    }
}
