using UnityEngine;

public class Snake : MonoBehaviour
{

    private const float MovementSpeed = 5.0f;
    private float RotationAnglesPerSecond = 180;
    private Rigidbody _rBody;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        _rBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftArrow)) 
        {
            // Rotate left 
            _rBody.MoveRotation(Quaternion.Euler(0, -RotationAnglesPerSecond * Time.deltaTime, 0) * transform.rotation);

        }
        else if (Input.GetKey(KeyCode.RightArrow)) 
        {
            // Rotate right
            _rBody.MoveRotation(Quaternion.Euler(0, RotationAnglesPerSecond * Time.deltaTime, 0) * transform.rotation);
        }

        // Move forward
        _rBody.MovePosition(transform.position + transform.forward * MovementSpeed * Time.deltaTime);
    }
}
