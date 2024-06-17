using UnityEngine;

public class Snake : MonoBehaviour
{

    [SerializeField]
    private GameObject snakeTailPrefab;

    private const float MovementSpeed = 5.0f;

    private float RotationSpeed = 360f;

    private int _tailStartSize = 5;

    private Transform _previousTail;

    void Awake()
    {
        GrowSnake(_tailStartSize);
    }

    private void GrowSnake(float growSize)
    {
        if(_previousTail == null)
            _previousTail = transform;

        for (int i = 0; i < growSize; i++)
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
