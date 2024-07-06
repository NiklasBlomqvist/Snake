using UnityEngine;

public class Tail : MonoBehaviour
{
    private Transform _targetTransform;
    private float _movementSpeed;
    private Vector3 _currentDirection;

    public void Init(Transform targetTransform, float movementSpeed, Color color)
    {
        _targetTransform = targetTransform;
        _movementSpeed = movementSpeed;
        GetComponentInChildren<Renderer>().material.SetColor("_BaseColor", color);
    }

    void Update()
    {
        // Create a ping pong effect on the movement speed - this will make the tail move faster and slower.
        var movementSpeedMultiplier = 1.3f + Mathf.PingPong(Time.time * 4f, 2f);

        if(_targetTransform != null)  
        {
            transform.position = Vector3.Lerp(transform.position, _targetTransform.position, Time.deltaTime * _movementSpeed * movementSpeedMultiplier);
            
            // Calculate movement direction
            _currentDirection = _targetTransform.position - transform.position;
            transform.LookAt(_targetTransform);
        }
        else 
        {
            // Continue moving in the last direction
            transform.position += _currentDirection.normalized * Time.deltaTime * _movementSpeed;
        }
    }
}
