using UnityEngine;

public class Tail : MonoBehaviour
{
    private const float MovementSpeed = 10f; 

    private Transform _targetTransform;
    private Vector3 _currentDirection;

    public void Init(Transform targetTransform)
    {
        _targetTransform = targetTransform;
    }

    void Update()
    {
        if(_targetTransform != null)  
        {
            transform.position = Vector3.Lerp(transform.position, _targetTransform.position, Time.deltaTime * MovementSpeed);
            
            // Calculate movement direction
            _currentDirection = _targetTransform.position - transform.position;
        }
        else 
        {
            // Continue moving in the last direction
            transform.position += _currentDirection.normalized * Time.deltaTime * MovementSpeed / 2;
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
