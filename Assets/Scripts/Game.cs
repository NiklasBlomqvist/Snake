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

    private string _gameOverMessage = "Game Over - Press SPACE to start a new game.";
    private string _startMessage = "Press SPACE to start a new game.";
    private string _pauseMessage = "Game Paused, Press ESC to resume.";

    private Snake _snake;
    private Treat _treat;

    private bool gamePaused;
    private bool _gameOver;

    void Awake()
    {
        mainMenu.ToggleMenu(_startMessage);
        Time.timeScale = mainMenu.IsMenuActive ? 0 : 1;
    }
     
    private void Update()
    {
        // Pause game.
        if (!_gameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            gamePaused = !gamePaused;
            mainMenu.ToggleMenu(_pauseMessage);
            Time.timeScale = mainMenu.IsMenuActive ? 0 : 1;
        } 
        // Start game.
        else if(!gamePaused && mainMenu.IsMenuActive && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
            mainMenu.ToggleMenu(_gameOverMessage);
        }
    }

    public void StartGame() 
    {
        _gameOver = false;
        Time.timeScale = 1;
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
        _gameOver = true;
        Destroy(_snake.gameObject);
        Destroy(_treat.gameObject);
        mainMenu.ToggleMenu(_gameOverMessage);
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
