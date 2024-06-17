using UnityEngine;

public class Tail : MonoBehaviour
{
    private Transform _targetTransform;
    private float _movementSpeed;
    private Vector3 _currentDirection;

    public void Init(Transform targetTransform, float movementSpeed)
    {
        _targetTransform = targetTransform;
        _movementSpeed = movementSpeed;
    }

    void Update()
    {
        if(_targetTransform != null)  
        {
            transform.position = Vector3.Lerp(transform.position, _targetTransform.position, Time.deltaTime * _movementSpeed * 2);
            
            // Calculate movement direction
            _currentDirection = _targetTransform.position - transform.position;
        }
        else 
        {
            // Continue moving in the last direction
            transform.position += _currentDirection.normalized * Time.deltaTime * _movementSpeed;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        var wall = other.GetComponent<Wall>();
        if (wall != null)
        {
            // Destroy tail when it hits a wall
            Destroy(gameObject);
        }        
    }
}
