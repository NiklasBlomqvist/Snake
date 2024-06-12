using UnityEngine;

public class Snake : MonoBehaviour
{

    private const float MovementSpeed = 8.0f;

    private Vector3 _moveDirection = Vector3.zero;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) 
        {
            _moveDirection = Vector3.forward;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) 
        {
            _moveDirection = Vector3.back;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) 
        {
            _moveDirection = Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) 
        {
            _moveDirection = Vector3.right;
        }

        transform.position += _moveDirection * MovementSpeed * Time.deltaTime;
    }
}
