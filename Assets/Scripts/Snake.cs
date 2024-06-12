using UnityEngine;

public class Snake : MonoBehaviour
{

    private const float MovementSpeed = 6.0f;
    private float RotationAnglesPerSecond = 180;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow)) 
        {
            transform.Rotate(Vector3.up, -RotationAnglesPerSecond * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow)) 
        {
            transform.Rotate(Vector3.up, RotationAnglesPerSecond * Time.deltaTime);
        }

        // Move forward
        transform.position += transform.forward * MovementSpeed * Time.deltaTime;
    }
}
