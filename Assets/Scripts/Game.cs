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

    [SerializeField]
    private GameObject butterflyPrefab;

    private const int TailStartSize = 3;

    private const int IgnoreTailCollision = 12;

    private const float SnakeMovementSpeed = 4.5f;

    private const float SnakeRotationSpeed = 320f;

    private const float OverlaySliderMultiplier = 2.0f;

    private const int  PupaeKnocksToTurnIntoButterfly = 3;

    private enum GameState
    {
        Larvae,
        Pupae,
        Butterfly
    }

    private Snake _snake;
    private List<Treat> _treats = new();
    private GameState _currentGameState;
    private Pupae _pupae;
    private Butterfly _butterfly;
    private bool _gamePaused;
    private bool _gameOver;
    private bool _gameStarted;
    private Coroutine _gameOverCoroutine;
    private List<SpawnPosition> _spawnPositions;
    private float _overlaySliderValue;
    private int _pupaeKnocks;
    
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

        HandleInput();

        UpdateOverlay();
    }

    private void UpdateOverlay()
    {
        if (_currentGameState == GameState.Larvae)
            overlay.SetFill(_overlaySliderValue);
    }

    private void TransformIntoPupae()
    {
        _currentGameState = GameState.Pupae;

        // Spawn pupae.
        _pupae = Instantiate(pupaePrefab, _snake.transform.position, Quaternion.identity).GetComponent<Pupae>();
        _pupae.Init(_snake.GetTails().ConvertAll(tail => tail.GetColor()));

        // Destroy snake and tails.
        var tails = _snake.GetTails();
        Destroy(_snake.gameObject);
        _snake = null;
        foreach (var tail in tails)
        {
            Destroy(tail.gameObject);
        }
    }

    private void TransformIntoButterfly()
    {
        _currentGameState = GameState.Butterfly;

        // Spawn butterfly.
        _butterfly = Instantiate(butterflyPrefab, _pupae.transform.position, butterflyPrefab.transform.rotation).GetComponent<Butterfly>();
        _butterfly.OnButterflyDissapeared += () =>
        {
            _currentGameState = GameState.Larvae;
            _pupaeKnocks = 0;
            _gameOver = true;
            _gameStarted = false;
            mainMenu.GameOverMenu();
        };

        // Destroy pupae.
        Destroy(_pupae.gameObject);
    }

    private void HandleInput()
    {
        // Start game.
        if (!_gamePaused && mainMenu.IsMenuActive && Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
            mainMenu.HideMenu();
        }

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

        // Space key to transform into pupae.
        if (!mainMenu.IsMenuActive && _currentGameState == GameState.Larvae)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                _overlaySliderValue = Mathf.Clamp(_overlaySliderValue += Time.deltaTime * OverlaySliderMultiplier, 0, 1);
            }
            else 
            {
                _overlaySliderValue = Mathf.Clamp(_overlaySliderValue -= Time.deltaTime * OverlaySliderMultiplier, 0, 1);
            }

            if (_overlaySliderValue >= 1)
            {
                TransformIntoPupae();
                _overlaySliderValue = 0f;
            }
        }
        if(!mainMenu.IsMenuActive && _currentGameState == GameState.Pupae) 
        {
            if(Input.GetKeyDown(KeyCode.Space)) 
            {
                _pupae.Knock();
                _pupaeKnocks++;

                if(_pupaeKnocks >= PupaeKnocksToTurnIntoButterfly) 
                {
                    TransformIntoButterfly();
                }
            }
        }

    }

    public void StartGame()
    {
        _gameOver = false;
        _gameStarted = true;
        SpawnSnake();

        foreach (var treat in _treats)
        {
            Destroy(treat.gameObject);
        }
        _treats = new();

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
