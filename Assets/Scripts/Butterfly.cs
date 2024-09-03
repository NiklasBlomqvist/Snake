using System;
using UnityEngine;

public class Butterfly : MonoBehaviour
{

    public Action OnButterflyDissapeared;

    private SkinnedMeshRenderer _skinnedMeshRenderer;

    private void Start()
    {
        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        Invoke(nameof(Dissapear), 5f);
    }

    void Update()
    {
        // Move forward 1 unit per second.
        var speed = 3f;
        transform.Translate(Vector3.up * Time.deltaTime * speed);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);

        // Loop betweeen 0 and 100 over duration 0.1 second.
        var wingSpeed = 0.08f;
        var wingBlendshape = Mathf.PingPong(Time.time / wingSpeed, 1);
        _skinnedMeshRenderer.SetBlendShapeWeight(0, wingBlendshape * 100);
    }

    private void Dissapear()
    {
        OnButterflyDissapeared?.Invoke();
        Destroy(gameObject);
    }
}
