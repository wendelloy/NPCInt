using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;  // Movement speed
    public float mouseSensitivity = 2f;  // Mouse sensitivity
    public Transform playerCamera;  // Reference to the camera for rotation
    public float verticalLookLimit = 80f;  // Limit for looking up/down
    public Transform playerBody;  // Reference to player body for rotation (if needed)

    private float rotationX = 0f;  // To control vertical camera rotation

    void Update()
    {
        // Handle player movement
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.Translate(moveX, 0, moveZ);  // Move the player in X and Z directions

        // Handle mouse input for camera rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate player (left/right) based on mouse movement
        if (playerBody != null)
        {
            playerBody.Rotate(0, mouseX, 0);
        }

        // Rotate camera (up/down) with limits
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -verticalLookLimit, verticalLookLimit);
        if (playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(rotationX, 0, 0);
        }
    }
}