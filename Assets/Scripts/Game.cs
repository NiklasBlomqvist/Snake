using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    [SerializeField]
    private Menu mainMenu;

    [SerializeField]
    private Overlay overlay;

    [SerializeField]
    private Transform SpawnPositionsParent;

    [SerializeField]
    private GameObject snakePrefab;

    [SerializeField]
    private GameObject treatPrefab;

    [SerializeField]
    private GameObject pupaePrefab;

    private const int TailStartSize = 3;

    private const int IgnoreTailCollision = 12;

    private const float SnakeMovementSpeed = 4.5f;

    private const float SnakeRotationSpeed = 320f;

    private Snake _snake;
    private List<Treat> _treats = new();

    private bool _gamePaused;
    private bool _gameOver;
    private bool _gameStarted;
    private Coroutine _gameOverCoroutine;
    private float _transformTimeAdded;
    private bool _transformToPupae;
    private List<SpawnPosition> _spawnPositions;
    private bool _becomeButterfly;
    private bool gamePupaePhase;

    void Awake()
    {
        _spawnPositions = new List<SpawnPosition>();
        for (int i = 0; i < SpawnPositionsParent.childCount; i++)
        {
            _spawnPositions.Add(SpawnPositionsParent.GetChild(i).GetComponent<SpawnPosition>());
        }
    }

    void Start()
    {
        mainMenu.StartMenu();
    }

    private void Update()
    {
        if (_gameOverCoroutine != null)
            return;

        if(gamePupaePhase)
        {
            return;
        }

        HandleInput();

        // Handle transform to butterfly.
        if (_becomeButterfly)
        {
            // Spawn pupae.
            var pupae = Instantiate(pupaePrefab, _snake.transform.position, Quaternion.identity).GetComponent<Pupae>();
            pupae.Init(_snake.GetTails().ConvertAll(tail => tail.GetColor()));

            // Destroy snake and tails.
            var tails = _snake.GetTails();
            Destroy(_snake.gameObject);
            _snake = null;
            foreach (var tail in tails)
            {
                Destroy(tail.gameObject);
            }

            // Enter pupae phase of the game.
            gamePupaePhase = true;
        }
        else
        {
            if (_transformToPupae)
            {
                if (_transformTimeAdded < 1f)
                    _transformTimeAdded += Time.deltaTime;

                if (_transformTimeAdded >= 1f)
                {
                    _becomeButterfly = true;
                }

                overlay.SetFill(_transformTimeAdded);
            }
            else
            {
                if (_transformTimeAdded > 0f)
                {
                    _transformTimeAdded -= Time.deltaTime;
                    overlay.SetFill(_transformTimeAdded);
                }
            }
        }
    }

    private void HandleInput()
    {
        // Pause game.
        if (!_gameOver && _gameStarted && Input.GetKeyDown(KeyCode.P))
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
        // Transform to pupae.
        else if (!mainMenu.IsMenuActive && Input.GetKeyDown(KeyCode.Space))
        {
            _transformToPupae = true;
        }
        else if (!mainMenu.IsMenuActive && Input.GetKeyUp(KeyCode.Space))
        {
            _transformToPupae = false;
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
        SpawnTreat(Treat.TreatColor.Red);
        SpawnTreat(Treat.TreatColor.Green);
        SpawnTreat(Treat.TreatColor.Blue);
    }

    private void SpawnSnake()
    {
        var spawnPosition = new Vector3(0, 1f, 0);
        _snake = Instantiate(snakePrefab, spawnPosition, Quaternion.identity).GetComponent<Snake>();
        _snake.Init(SnakeMovementSpeed, SnakeRotationSpeed, TailStartSize, IgnoreTailCollision);
        _snake.GameOver += OnGameOver;
        _snake.EatTreat += OnEatTreat;
    }

    private void OnGameOver()
    {
        Debug.LogError("Game Over");

        _gameOver = true;
        _gameStarted = false;

        var tails = _snake.GetTails();

        _gameOverCoroutine = StartCoroutine(GameOverRoutine(tails, _snake, _treats));
    }

    private IEnumerator GameOverRoutine(List<Tail> tails, Snake snake, List<Treat> treats)
    {
        Destroy(snake.gameObject);

        foreach (var treat in treats)
        {
            Destroy(treat.gameObject);
        }

        foreach (var tail in tails)
        {
            Destroy(tail.gameObject);
            yield return new WaitForSeconds(0.05f);
        }

        mainMenu.GameOverMenu();
        _gameOverCoroutine = null;
    }

    private void OnEatTreat(Treat treat)
    {
        // Grow snake
        _snake.GrowSnake(1, treat.GetColor());

        _treats.Remove(treat);
        SpawnTreat(treat.CurrentColor);
        treat.CurrentSpawnPosition.IsOccupied = false;

        Destroy(treat.gameObject);
    }

    private void SpawnTreat(Treat.TreatColor treatColor)
    {
        var ranmdomSpawnPosition = GetRandomSpawnPosition();
        var treat = Instantiate(treatPrefab, ranmdomSpawnPosition.transform.position, Quaternion.identity).GetComponent<Treat>();
        treat.SetColor(treatColor);
        treat.SetSpawnPosition(ranmdomSpawnPosition);
        ranmdomSpawnPosition.IsOccupied = true;
        _treats.Add(treat);
    }

    private SpawnPosition GetRandomSpawnPosition()
    {
        // Get all available spawn positions.
        var availableSpawnPositions = _spawnPositions.FindAll(sp => !sp.IsOccupied);

        // Create list of all available spawn positions that is not close to the snake.
        var minDistanceFromSnake = 1.5f;
        var snakeTailPositions = _snake.GetSnakeTransforms();
        foreach (var tail in snakeTailPositions)
        {
            availableSpawnPositions = availableSpawnPositions.FindAll(sp => Vector3.Distance(sp.transform.position, tail.position) > minDistanceFromSnake);
        }

        // Get random spawn position from available spawn positions.
        var randomIndex = Random.Range(0, availableSpawnPositions.Count);
        var randomSpawnPosition = availableSpawnPositions[randomIndex];

        return randomSpawnPosition;
    }
}
