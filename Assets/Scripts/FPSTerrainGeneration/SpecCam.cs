using UnityEngine;

public class SpecCam : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust the speed as needed
    public float verticalSpeed = 3f; // Adjust the vertical movement speed as needed

    void Update()
    {
        // Get the user input for movement
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        // Calculate the movement direction based on the camera's forward and right vectors
        Vector3 moveDirection = (transform.forward * verticalMovement + transform.right * horizontalMovement).normalized;
        //transform.position = transform.position + new Vector3(horizontalInput * movementSpeed * Time.deltaTime, verticalInput * movementSpeed * Time.deltaTime, 0);

        // Move the camera in the calculated direction
        //transform.position.x += (moveDirection.x * moveSpeed * Time.deltaTime);
        //transform.position.z += (moveDirection.z * moveSpeed * Time.deltaTime);
        transform.position = transform.position + new Vector3(moveDirection.x * moveSpeed * Time.deltaTime, 0, moveDirection.z * moveSpeed * Time.deltaTime);

        //transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // Move the camera up when holding 'E'
        if (Input.GetKey(KeyCode.E))
        {
            //transform.position.y += (Vector3.up * verticalSpeed * Time.deltaTime);
            //transform.Translate(Vector3.up * verticalSpeed * Time.deltaTime, Space.World);
        }

        // Move the camera down when holding 'Q'
        if (Input.GetKey(KeyCode.Q))
        {
            //transform.position.y += (Vector3.down * verticalSpeed * Time.deltaTime);
        }
    }
}
