using System;
using UnityEngine;

public class Game : MonoBehaviour
{

    [SerializeField]
    private GameObject snakePrefab;

    [SerializeField]
    private GameObject treatPrefab;

    private int _boardSize = 5;
    private Snake _snake;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        SpawnSnake();
        SpawnTreat();
    }

    private void SpawnSnake()
    {
        // Instatiate snake in random position within board
        var minOffsetFromBorder = 2f;
        _snake = Instantiate(snakePrefab, new Vector3(UnityEngine.Random.Range(-_boardSize + minOffsetFromBorder, _boardSize - minOffsetFromBorder), 0, UnityEngine.Random.Range(-_boardSize + minOffsetFromBorder, _boardSize - minOffsetFromBorder)), Quaternion.identity).GetComponent<Snake>();
    }

    private void SpawnTreat()
    {
        
    }
}
