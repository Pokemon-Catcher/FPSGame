using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float rotationSpeed = 10f;
    public float jumpSpeed = 10f;
    public float gravity = 9.8f;
    public float speedPerPixel = 1f;
    public float slowConstant = 0.1f;
    public float slowSpeedApplier = 0.9f;
    public float stepInterval = 0.8f;
    public AudioSource audioSource;
    public AudioClip[] stepSounds;
    private float stepTimer = 0;
    private Vector3 mousePosPrevious;
    private Vector3 acceleration = Vector3.zero;
    private Transform parentTrans;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private Vector3 deltaMousePosition = Vector3.zero;
    private Vector3 previousMousePosition = Vector3.zero;
    private Vector3 mousePosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponentInParent <CharacterController>() ;
        mousePosPrevious = Input.mousePosition;
        Cursor.visible = false;
        parentTrans = controller.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseDelta = Vector3.zero;
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            mousePosPrevious = Input.mousePosition;
        }
        if (Input.mousePosition.x >= Screen.width || Input.mousePosition.y >= Screen.height || Input.mousePosition.x <= 0 || Input.mousePosition.y <= 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            mouseDelta = Input.mousePosition - mousePosPrevious;
        }
        acceleration += new Vector3(-mouseDelta.y * speedPerPixel, 0, mouseDelta.x * speedPerPixel);
        acceleration = new Vector3(Mathf.Max(Mathf.Abs(acceleration.x * slowSpeedApplier) - slowConstant, 0) * Mathf.Sign(acceleration.x), 0, Mathf.Max(Mathf.Abs(acceleration.z * slowSpeedApplier) - slowConstant, 0) * Mathf.Sign(acceleration.z));
        transform.Rotate(acceleration.x, 0, 0);
        parentTrans.Rotate(0, acceleration.z, 0);
        mousePosPrevious = Input.mousePosition;
        
        if (controller.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection = parentTrans.TransformDirection(moveDirection);
            moveDirection = moveDirection * speed;
            
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity
        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);
        if (stepTimer >= stepInterval)
        {
            stepTimer = 0;
        } else if (stepTimer == 0 && moveDirection.x != 0 && moveDirection.z != 0 && controller.isGrounded)
        {
            audioSource.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Length - 1)]);
            stepTimer += 0.01f;
        } else if (stepTimer > 0)
        {
            stepTimer += Time.deltaTime;
        }
        // Move the controller
        controller.Move(moveDirection * Time.deltaTime);
    }
}
