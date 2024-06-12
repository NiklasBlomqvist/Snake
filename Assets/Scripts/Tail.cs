using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    private const float MovementSpeed = 5f; 

    private Transform _targetTransform;

    public void Init(Transform targetTransform)
    {
        _targetTransform = targetTransform;
    }

    void Update()
    {
        // Move forward
        transform.position += transform.forward * MovementSpeed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(_targetTransform.position - transform.position);

        // transform.position = Vector3.Lerp(transform.position, _targetTransform.position, Time.deltaTime * MovementSpeed);
    }
}
