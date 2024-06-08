using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    [SerializeField]
    private GameObject treatPrefab;

    private readonly int _boardSize = 10;

    void Start()
    {
        SpawnTreat();
    }

    private void SpawnTreat()
    {
        var spawnPosition = GetRandomPosition();
        Instantiate(treatPrefab, spawnPosition, Quaternion.identity);
    }

    private Vector3 GetRandomPosition()
    {
        // Create a random position in 0,5, 1,5, 2,5, 3,5, 4,5, 5,5, 6,5, 7,5, 8,5, 9,5
        var randomPos = UnityEngine.Random.Range(0, _boardSize/2) + 0.5f;
        return new Vector3(randomPos, 0f, randomPos);
    }
}
