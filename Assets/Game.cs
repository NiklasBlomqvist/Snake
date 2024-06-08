using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    private GameObject snakePrefab;

    [SerializeField]
    private GameObject treatPrefab;

    private readonly int _boardSize = 5; // [-boardSize, boardSize]

    void Start()
    {
        SpawnSnake();
        SpawnTreat();
    }

    private void SpawnSnake()
    {
        var spawnPosition = GetRandomPosition();
        Instantiate(snakePrefab, spawnPosition, Quaternion.identity);
    }

    private void SpawnTreat()
    {
        var spawnPosition = GetRandomPosition();
        Instantiate(treatPrefab, spawnPosition, Quaternion.identity);
    }

    private Vector3 GetRandomPosition()
    {
        var randomPos = UnityEngine.Random.Range(0, _boardSize) + 0.5f;
        return new Vector3(randomPos, 0f, randomPos);
    }
}
