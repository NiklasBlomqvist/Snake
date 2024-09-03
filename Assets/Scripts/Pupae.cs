using System;
using System.Collections;
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

    public void Knock() 
    {
        StartCoroutine(KnockCoroutine());
    }

    private IEnumerator KnockCoroutine()
    {
        var startScale = transform.localScale;
        var scalePhaseOne = startScale * 1.2f;
        var scalePhaseTwo = startScale * 0.8f;

        var durationPerPhase = 0.2f;

        for (float t = 0; t < 1; t += Time.deltaTime / durationPerPhase)
        {
            transform.localScale = Vector3.Lerp(startScale, scalePhaseOne, t);
            yield return null;
        }

        for (float t = 0; t < 1; t += Time.deltaTime / durationPerPhase)
        {
            transform.localScale = Vector3.Lerp(scalePhaseOne, scalePhaseTwo, t);
            yield return null;
        }

        transform.localScale = startScale;
    }
}
