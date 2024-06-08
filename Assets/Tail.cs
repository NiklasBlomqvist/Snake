using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{

    private Transform _target;
    private Vector3 _previousTargetPos;

    public void Follow(Transform target)
    {
        _target = target;
        _previousTargetPos = target.position;
    }

    public void Tick()
    {
        if (_target == null)
        {
            return;
        }

        transform.position = _previousTargetPos;
        _previousTargetPos = _target.position;
    }
}
