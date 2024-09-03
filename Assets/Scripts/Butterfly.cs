using UnityEngine;

public class Butterfly : MonoBehaviour
{
    private SkinnedMeshRenderer _skinnedMeshRenderer;

    void Awake()
    {
        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
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
}
