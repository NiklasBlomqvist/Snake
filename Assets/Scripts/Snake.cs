using System;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{

    [SerializeField]
    private GameObject snakeTailPrefab;

    public Action EatTreat;

    private float RotationSpeed = 360f;

    private int _tailStartSize = 5;

    private List<Tail> _tails = new List<Tail>();

    private Transform _previousTail;
    private Transform _tailTransform;
    private float _movementSpeed;

    public void Init(float movementSpeed)
    {
        _movementSpeed = movementSpeed;
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
        transform.Translate(Vector3.forward * _movementSpeed * Time.deltaTime);
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
            // Destroy snake
            Destroy(gameObject);
            Debug.LogError("Game Over!");
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
                    // Destroy snake
                    Destroy(gameObject);
                    Debug.LogError("Game Over!");
                    break;
                }
            }
        }
    }
}
