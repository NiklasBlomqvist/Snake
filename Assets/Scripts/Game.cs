using System;
using System.Collections;
using System.Collections.Generic;
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

    private const int BoardSize = 5;

    private const float SnakeMovementSpeed = 5.0f;

    private const float SnakeRotationSpeed = 450f;

    private const float SnakeSlowMotionTimeScale = 0.5f;

    private const int TailStartSize = 5;

    private Snake _snake;
    private Treat _treat;

    private bool _gamePaused;
    private bool _gameOver;
    private bool _gameStarted;
    private Coroutine _gameOverCoroutine;

    void Start()
    {
        mainMenu.StartMenu();
    }
     
    private void Update()
    {
        if(_gameOverCoroutine != null)
            return;

        HandleInput();
    }

    private void HandleInput()
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
        // Slowmo
        else if(!mainMenu.IsMenuActive && Input.GetKey(KeyCode.Space)) 
        {
            Time.timeScale = SnakeSlowMotionTimeScale;
        }
        else if(!mainMenu.IsMenuActive && Input.GetKeyUp(KeyCode.Space)) 
        {
            Time.timeScale = 1f;
        }
        // Start game.
        else if (!_gamePaused && mainMenu.IsMenuActive && Input.GetKeyDown(KeyCode.Space))
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
        var minOffsetFromBorder = 4f;
        var spawnPosition = new Vector3(UnityEngine.Random.Range(-BoardSize + minOffsetFromBorder, BoardSize - minOffsetFromBorder), snakePrefab.transform.position.y, UnityEngine.Random.Range(-BoardSize + minOffsetFromBorder, BoardSize - minOffsetFromBorder));
        _snake = Instantiate(snakePrefab, spawnPosition, Quaternion.identity).GetComponent<Snake>();
        _snake.Init(SnakeMovementSpeed, SnakeRotationSpeed, TailStartSize);
        _snake.GameOver += OnGameOver;
        _snake.EatTreat += OnEatTreat;
    }

    private void OnGameOver()
    {
        Debug.LogError("Game Over");

        _gameOver = true;
        _gameStarted = false;

        var tails = _snake.GetTails();

        _gameOverCoroutine = StartCoroutine(GameOverRoutine(tails, _snake, _treat));
    }

    private IEnumerator GameOverRoutine(List<Tail> tails, Snake snake, Treat treat)
    {
        Destroy(snake.gameObject);
        Destroy(treat.gameObject);

        foreach (var tail in tails)
        {
            Destroy(tail.gameObject);
            yield return new WaitForSeconds(0.05f);
        }

        mainMenu.GameOverMenu();
        _gameOverCoroutine = null;
    }

    private void OnEatTreat()
    {
        SpawnTreat();
    }

    private void SpawnTreat()
    {
        var minOffsetFromBorder = 1f;
        var minDistanceFromSnake = 1f;
        
        // random position that is not on the snake
        Vector3 spawnPosition;
        do
        {
            spawnPosition = new Vector3(UnityEngine.Random.Range(-BoardSize + minOffsetFromBorder, BoardSize - minOffsetFromBorder), treatPrefab.transform.position.y, UnityEngine.Random.Range(-BoardSize + minOffsetFromBorder, BoardSize - minOffsetFromBorder));
        } while (_snake.GetSnakeTransforms().Exists(s => Vector3.Distance(spawnPosition, s.position) < minDistanceFromSnake));
        
        _treat = Instantiate(treatPrefab, spawnPosition, Quaternion.identity).GetComponent<Treat>();
    }
}
