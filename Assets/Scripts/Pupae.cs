using System.Collections.Generic;
using UnityEngine;

public class Pupae : MonoBehaviour
{
    private List<Color> _colors;

    public void Init(List<Color> colors) 
    {
        _colors = colors;

        Debug.Log("Pupae initialized with " + _colors.Count + " colors.");
        Debug.Log("Colors: " + string.Join(", ", _colors));
    }
}
