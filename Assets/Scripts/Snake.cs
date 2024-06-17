using UnityEngine;

public class Snake : MonoBehaviour
{

    [SerializeField]
    private GameObject snakeTailPrefab;

    private const float MovementSpeed = 5.0f;

    private float RotationSpeed = 360f;

    private int _tailSize = 10;

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
        // Handle input.
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // Rotate angle left
            transform.Rotate(Vector3.up, -RotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            // Rotate angle right
            transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
        }

        // Move forward
        transform.Translate(Vector3.forward * MovementSpeed * Time.deltaTime);
    }
}
