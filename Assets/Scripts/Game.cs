using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SnakeGame
{
    public class Game : MonoBehaviour
    {

        [SerializeField]
        private Board board;
        [SerializeField]
        private GameObject snakePrefab;
        [SerializeField]
        private GameObject treatPrefab;

        private const float TickSpeed = 0.15f; // How often tick and move happens.
        private const float MovementPerTick = 1.0f; // How much the snake moves per tick.
        private const int BoardSize = 14; // Number of tiles in one axis of the board. The board is a square (_boardSize * _boardSize), starting at bottom left corner (0, 0).

        private bool _isGameOver = false;
        private Dictionary<Vector3, bool> _snakePositions;
        private Snake _snake;
        private Vector3 _previousDirection;

        void Start()
        {
            board.Initialize(BoardSize);

            InitializeSnakePositions();

            SpawnSnake(MovementPerTick);
            SpawnTreat();

            StartCoroutine(Tick());
        }

        private void InitializeSnakePositions()
        {
            _snakePositions = new Dictionary<Vector3, bool>(BoardSize * BoardSize);
            for (int x = 0; x < BoardSize; x++)
            {
                for (int z = 0; z < BoardSize; z++)
                {
                    _snakePositions.Add(new Vector3(x, 0, z), false);
                }
            }
        }

        private IEnumerator Tick()
        {
            while (!_isGameOver)
            {
                yield return new WaitForSeconds(TickSpeed);

                UpdateSnakePositions();

                // Check if next move will hit a wall or itself
                var nextMove = Input.Instance.NextDirection;
                if(nextMove == -_previousDirection) 
                {
                    nextMove = _previousDirection;
                }
                if (IsNextMoveValid(nextMove))
                {
                    _snake.Tick(nextMove);
                }
                else
                {
                    _isGameOver = true;
                    Debug.Log("Game Over!");
                }
                _previousDirection = nextMove;
            }
        }

        private void UpdateSnakePositions()
        {
            InitializeSnakePositions();

            _snakePositions[_snake.GetHeadPosition()] = true;
            foreach (var tailPosition in _snake.GetTailPositions())
            {
                _snakePositions[tailPosition] = true;
            }
        }

        private bool IsNextMoveValid(Vector3 nextDirection)
        {
            if(nextDirection == Vector3.zero)    
                return true;

            var nextPosition = _snake.GetHeadPosition() + nextDirection * MovementPerTick;

            // Check if next position is outside the board.
            if (nextPosition.x < 0 || nextPosition.x >= BoardSize || nextPosition.z < 0 || nextPosition.z >= BoardSize)
            {
                return false;
            }

            // // Check if next position is occupied.
            if (_snake.GetTailPositions().Contains(nextPosition))
            {
                return false;
            }

            return true;
        }

        private void SpawnSnake(float movementPerTick)
        {
            var spawnPosition = GetRandomUnoccupiedPositonBySnake();
            _snake = Instantiate(snakePrefab, spawnPosition, Quaternion.identity).GetComponent<Snake>();
            _snake.Initialize(movementPerTick);
            _snake.EatTreat += SpawnTreat;
        }

        private void SpawnTreat()
        {
            var spawnPosition = GetRandomUnoccupiedPositonBySnake();
            Instantiate(treatPrefab, spawnPosition, Quaternion.identity);
        }

        private Vector3 GetRandomUnoccupiedPositonBySnake()
        {
            // Get all Vector2 types from the dictionary where the value is false.
            var unoccupiedPositions = _snakePositions.Where(x => x.Value == false).Select(x => x.Key).ToList();

            // Get a random position from the list of unoccupied positions.
            var randomIndex = Random.Range(0, unoccupiedPositions.Count);
            var randomPosition = unoccupiedPositions[randomIndex];

            return randomPosition;
        }
    }
}