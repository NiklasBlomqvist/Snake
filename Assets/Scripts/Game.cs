using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SnakeGame
{
    public class Game : MonoBehaviour
    {
        [SerializeField]
        private GameObject snakePrefab;

        [SerializeField]
        private GameObject treatPrefab;

        private float TickSpeed = 0.2f; // How often tick and move happens.
        private float MovementPerTick = 1.0f; // How much the snake moves per tick.

        private bool _isGameOver = false;

        private readonly int _boardSize = 5; // [-boardSize, boardSize]

        private List<Vector3> _boardPositions = new List<Vector3>();
        private List<Vector3> _freeBoardPositions = new List<Vector3>();
        private List<Vector3> _snakeBoardPositions = new List<Vector3>();

        private Snake _snake;
        private Vector3 _currentTreatPositon;

        void Start()
        {
            InitializeBoardPositions();

            SpawnSnake(MovementPerTick);
            SpawnTreat();

            StartCoroutine(Tick());
        }

        private void InitializeBoardPositions()
        {
            for (int x = -_boardSize; x < _boardSize; x++)
            {
                for (int z = -_boardSize; z < _boardSize; z++)
                {
                    _boardPositions.Add(new Vector3(x + 0.5f, 0f, z + 0.5f));
                }
            }

            _freeBoardPositions = new List<Vector3>(_boardPositions);
        }

        private IEnumerator Tick()
        {
            while (!_isGameOver)
            {
                yield return new WaitForSeconds(TickSpeed);

                UpdateAvailableBoardPositions();

                // Check if next move will hit a wall or itself
                var nextMove = Input.Instance.CurrentDirection;
                if (IsNextMoveValid(nextMove))
                {
                    _snake.Tick(nextMove);
                }
                else
                {
                    _isGameOver = true;
                    Debug.Log("Game Over!");
                }
            }
        }

        private void UpdateAvailableBoardPositions()
        {
            // Remove occupied positions from the list of empty board positions.
            var occupiedPositions = new List<Vector3>();
            _snakeBoardPositions.Clear();
            if (_snake != null) 
            {
                _snakeBoardPositions = _snake.GetAllOccupiedPositions();
                occupiedPositions.AddRange(_snakeBoardPositions);
            }
            if (_currentTreatPositon != Vector3.zero)
                occupiedPositions.Add(_currentTreatPositon);

            // Log all occupied positions.
            Debug.Log("Occupied positions:");
            foreach (var pos in occupiedPositions)
            {
                Debug.Log(pos);
            }

            _freeBoardPositions = _boardPositions.Except(occupiedPositions).ToList();
        }

        private bool IsNextMoveValid(Input.MovementDirection currentDirection)
        {
            if(currentDirection == Input.MovementDirection.None)    
                return true;

            Debug.Log($"Is Next Move Valid? {currentDirection}");

            var nextPosition = _snake.transform.position;

            Debug.Log($"Current position: {_snake.transform.position}");

            switch (currentDirection)
            {
                case Input.MovementDirection.Up:
                    nextPosition += Vector3.forward * MovementPerTick;
                    break;
                case Input.MovementDirection.Down:
                    nextPosition += Vector3.back * MovementPerTick;
                    break;
                case Input.MovementDirection.Left:
                    nextPosition += Vector3.left * MovementPerTick;
                    break;
                case Input.MovementDirection.Right:
                    nextPosition += Vector3.right * MovementPerTick;
                    break;
                case Input.MovementDirection.None:
                    break;
            }

            Debug.Log($"Next position: {nextPosition}");

            // Check if next position is outside the board.
            if (nextPosition.x < -_boardSize || nextPosition.x >= _boardSize || nextPosition.z < -_boardSize || nextPosition.z >= _boardSize)
            {
                Debug.Log($"Hit wall at next position {nextPosition}.");
                return false;
            }

            // // Check if next position is occupied.
            if (_snakeBoardPositions.Contains(nextPosition))
            {
                Debug.Log($"Hit itself at next position {nextPosition}.");
                return false;
            }

            return true;
        }

        private void SpawnSnake(float movementPerTick)
        {
            var spawnPosition = GetRandomUnoccupiedPosition();
            _snake = Instantiate(snakePrefab, spawnPosition, Quaternion.identity).GetComponent<Snake>();
            _snake.Initialize(movementPerTick);
            _snake.EatTreat += SpawnTreat;
        }

        private void SpawnTreat()
        {
            var spawnPosition = GetRandomUnoccupiedPosition();
            Instantiate(treatPrefab, spawnPosition, Quaternion.identity);
            _currentTreatPositon = spawnPosition;
        }

        private Vector3 GetRandomUnoccupiedPosition()
        {
            if(_freeBoardPositions.Count == 0)
                throw new Exception("No free board positions left.");

            // Pick random unoccupied position from the list of empty board positions.
            var randomIndex = UnityEngine.Random.Range(0, _freeBoardPositions.Count);
            var randomPosition = _freeBoardPositions[randomIndex];

            return randomPosition;
        }
    }
}