using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Snake : MonoBehaviour
{

    public Action EatTreat;

    [SerializeField]
    private GameObject tailPrefab;


    private Vector3 nextTailPosition;

    private float _movementPerTick;

    private List<Tail> _tails = new List<Tail>();

    private MovementDirection CurrentDirection = MovementDirection.None;


    private enum MovementDirection
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    public void Initialize(float movementPerTick)
    {
        _movementPerTick = movementPerTick;
    }
    public void Tick()
    {
        Move();

        foreach (var tail in _tails)
        {
            tail.Tick();
        }
    }

    private void Update()
    {
        // Handle input.
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            CurrentDirection = MovementDirection.Up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            CurrentDirection = MovementDirection.Down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CurrentDirection = MovementDirection.Left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            CurrentDirection = MovementDirection.Right;
        }
    }

    private void Move()
    {
        switch (CurrentDirection)
        {
            case MovementDirection.Up:
                transform.position += Vector3.forward * _movementPerTick;
                nextTailPosition = transform.position + Vector3.back * _movementPerTick;
                break;
            case MovementDirection.Down:
                transform.position += Vector3.back * _movementPerTick;
                nextTailPosition = transform.position + Vector3.forward * _movementPerTick;
                break;
            case MovementDirection.Left:
                transform.position += Vector3.left * _movementPerTick;
                nextTailPosition = transform.position + Vector3.right * _movementPerTick;
                break;
            case MovementDirection.Right:
                transform.position += Vector3.right * _movementPerTick;
                nextTailPosition = transform.position + Vector3.left * _movementPerTick;
                break;
            case MovementDirection.None:
                break;
        }
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        var treat = other.GetComponentInParent<Treat>();
        if (treat == null) return;
        Eat(treat);
    }

    private void Eat(Treat treat)
    {
        treat.GetEaten();

        Grow();

        EatTreat?.Invoke();
    }

    private void Grow()
    {
        var tail = Instantiate(tailPrefab, new Vector3(nextTailPosition.x, tailPrefab.transform.position.y, nextTailPosition.z), Quaternion.identity).GetComponent<Tail>();
        

        // If there are no tails, make the tail follow the head. Otherwise, make the tail follow the last tail.
        if(_tails.Count < 1) 
        {
            tail.Follow(transform);
        }
        else 
        {
            tail.Follow(_tails[_tails.Count - 1].transform);
        }

        _tails.Add(tail);
    }
}
