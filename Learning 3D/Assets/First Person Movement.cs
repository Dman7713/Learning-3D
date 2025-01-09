using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform playerCamera;
    public float mouseSensitivity = 100f;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public LayerMask groundLayer;

    [Header("Gravity Settings")]
    public float gravity = -9.81f;

    private float xRotation = 0f;
    private Vector3 velocity;
    private CharacterController controller;
    private bool isGrounded;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the screen center
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        CameraRotation();
        PlayerMovement();
    }

    void CameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit vertical look

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void PlayerMovement()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundLayer);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Movement input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
