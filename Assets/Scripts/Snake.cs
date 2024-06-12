using UnityEngine;

public class Snake : MonoBehaviour
{

    [SerializeField]
    private GameObject snakeTailPrefab;

    private const float MovementSpeed = 5.0f;

    private float RotationAnglesPerSecond = 180;

    private int _tailSize = 100;

    private Transform _previousTail;

    void Awake()
    {
        _previousTail = transform;

        for (int i = 0; i < _tailSize; i++)
        {
            var tail = Instantiate(snakeTailPrefab, _previousTail.position, Quaternion.identity).GetComponent<Tail>();
            
            tail.Init(_previousTail);

            _previousTail = tail.transform;
        }

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // Rotate left 
            transform.Rotate(0, -RotationAnglesPerSecond * Time.deltaTime, 0);

        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            // Rotate right
            transform.Rotate(0, RotationAnglesPerSecond * Time.deltaTime, 0);
        }

        // Move forward
        transform.Translate(Vector3.forward * MovementSpeed * Time.deltaTime);
    }
}
