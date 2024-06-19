using System;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{

    [SerializeField]
    private GameObject snakeTailPrefab;

    public Action EatTreat;
    public Action GameOver;

    private List<Transform> _snakeTransform = new List<Transform>();
    private List<Tail> _tails = new List<Tail>();

    private Transform _previousTail;
    private Transform _tailTransform;
    private float _movementSpeed;
    private float _rotationSpeed;
    private int _tailStartSize;

    public void Init(float movementSpeed, float rotationSpeed, int tailStartSize)
    {
        _movementSpeed = movementSpeed;
        _rotationSpeed = rotationSpeed;
        _tailStartSize = tailStartSize;

        _snakeTransform.Add(transform); // Add head to snake positions.

        GrowSnake(_tailStartSize);
    }

    private void GrowSnake(float growSize)
    {
        if(_previousTail == null) 
        {
            _previousTail = transform;
            
            // Create empy game object to hold the tail.
            _tailTransform = new GameObject("Tail").transform;
        }

        for (int i = 0; i < growSize; i++)
        {
            var tail = Instantiate(snakeTailPrefab, _previousTail.position, Quaternion.identity, _tailTransform).GetComponent<Tail>();
            tail.Init(_previousTail, _movementSpeed);
            _tails.Add(tail);
            _snakeTransform.Add(tail.transform);
            _previousTail = tail.transform;
        }
    }

    void Update()
    {
        // Handle input.
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // Rotate angle left
            transform.Rotate(Vector3.up, -_rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            // Rotate angle right
            transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
        }

        // Move forward
        transform.Translate(Vector3.forward * _movementSpeed * Time.deltaTime);
    }

    public List<Tail> GetTails()
    {
        return _tails;
    }
    
    public List<Transform> GetSnakeTransforms()
    {
        return _snakeTransform;
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        var treat = other.GetComponentInParent<Treat>();
        if(treat != null)
        {
            // Destroy treat
            Destroy(treat.gameObject);

            // Grow snake
            GrowSnake(1);

            EatTreat?.Invoke();
        }

        var wall = other.GetComponent<Wall>();
        if(wall != null)
        {
            GameOver?.Invoke();
        }

        var collidedTail = other.GetComponent<Tail>();
        if(collidedTail != null)
        {
            // Check if tail is part of the first instantiated tails
            for (int i = 0; i < _tails.Count; i++)
            {
                var tail = _tails[i];
                if(i <= _tailStartSize)
                {
                    continue;
                }
                else if(tail == collidedTail)
                {
                    GameOver?.Invoke();
                    break;
                }
            }
        }
    }
}
