using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Game : MonoBehaviour
{

    [SerializeField]
    private Menu mainMenu;

    [SerializeField]
    private GameObject snakePrefab;

    [SerializeField]
    private GameObject treatPrefab;

    private int _boardSize = 5;

    private float SnakeMovementSpeed = 5.0f;

    private Snake _snake;
    private Treat _treat;

    private bool _gamePaused;
    private bool _gameOver;
    private bool _gameStarted;

    void Start()
    {
        mainMenu.StartMenu();
    }
     
    private void Update()
    {
        // Pause game.
        if (!_gameOver && _gameStarted && Input.GetKeyDown(KeyCode.Escape))
        {
            _gamePaused = !_gamePaused;
            if (_gamePaused)
            {
                mainMenu.PauseMenu();
            }
            else
            {
                mainMenu.HideMenu();
            }
        } 
        // Start game.
        else if(!_gamePaused && mainMenu.IsMenuActive && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
            mainMenu.HideMenu();
        }
    }

    public void StartGame() 
    {
        _gameOver = false;
        _gameStarted = true;
        SpawnSnake();
        SpawnTreat();
    }

    private void SpawnSnake()
    {
        // Instatiate snake in random position within board
        var minOffsetFromBorder = 2f;
        var spawnPosition = new Vector3(UnityEngine.Random.Range(-_boardSize + minOffsetFromBorder, _boardSize - minOffsetFromBorder), snakePrefab.transform.position.y, UnityEngine.Random.Range(-_boardSize + minOffsetFromBorder, _boardSize - minOffsetFromBorder));
        _snake = Instantiate(snakePrefab, spawnPosition, Quaternion.identity).GetComponent<Snake>();
        _snake.Init(SnakeMovementSpeed);
        _snake.GameOver += OnGameOver;
        _snake.EatTreat += OnEatTreat;
    }

    private void OnGameOver()
    {
        Debug.LogError("Game Over");

        _gameOver = true;
        _gameStarted = false;
        Destroy(_snake.gameObject);
        Destroy(_treat.gameObject);

        mainMenu.GameOverMenu();
    }

    private void OnEatTreat()
    {
        SpawnTreat();
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
