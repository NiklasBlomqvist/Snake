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
    private Treat _treat;

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
        var spawnPosition = new Vector3(UnityEngine.Random.Range(-_boardSize + minOffsetFromBorder, _boardSize - minOffsetFromBorder), snakePrefab.transform.position.y, UnityEngine.Random.Range(-_boardSize + minOffsetFromBorder, _boardSize - minOffsetFromBorder));
        _snake = Instantiate(snakePrefab, spawnPosition, Quaternion.identity).GetComponent<Snake>();
    }

    private void SpawnTreat()
    {
        var minOffsetFromBorder = 1f;
        
        // random position that is not on the snake
        Vector3 spawnPosition;
        do
        {
            spawnPosition = new Vector3(UnityEngine.Random.Range(-_boardSize + minOffsetFromBorder, _boardSize - minOffsetFromBorder), treatPrefab.transform.position.y, UnityEngine.Random.Range(-_boardSize + minOffsetFromBorder, _boardSize - minOffsetFromBorder));
        } while (Vector3.Distance(spawnPosition, _snake.transform.position) < 1f);
        
        _treat = Instantiate(treatPrefab, spawnPosition, Quaternion.identity).GetComponent<Treat>();
    }
}
