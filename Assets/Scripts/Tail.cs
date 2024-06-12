using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    private Transform _targetTransform;

    public void Init(Transform targetTransform)
    {
        _targetTransform = targetTransform;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _targetTransform.position, Time.deltaTime * 5f);
    }
}
