using UnityEngine;

public class Treat : MonoBehaviour
{

    public enum TreatColor 
    {
        Red,
        Green,
        Blue
    }

    public TreatColor CurrentColor { get; private set; }

    public SpawnPosition CurrentSpawnPosition { get; private set; }

    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
    }

    public void SetSpawnPosition(SpawnPosition spawnPosition) 
    {
        CurrentSpawnPosition = spawnPosition;
    }

    public void SetColor(TreatColor color)
    {
        CurrentColor = color;

        switch(color)
        {
            case TreatColor.Red:
                _renderer.material.SetColor("_BaseColor", Color.red);
                break;
            case TreatColor.Green:
                _renderer.material.SetColor("_BaseColor", Color.green);
                break;
            case TreatColor.Blue:
                _renderer.material.SetColor("_BaseColor", Color.blue);
                break;
        }
    }
}
