using System;
using System.Collections;
using System.Collections.Generic;
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

        private List<Vector3> _boardPositions = new List<Vector3>(); // X and Y is the positions, Z is if it is occupied

        private Snake _snake;

        void Start()
        {
            // Initialize the board positions
            for (int x = 0; x < _boardSize; x++)
            {
                for (int z = 0; z < _boardSize; z++)
                {
                    _boardPositions.Add(new Vector3(x + 0.5f, z + 0.5f, 0f));
                }
            }

            SpawnSnake(MovementPerTick);
            SpawnTreat();

            StartCoroutine(Tick());
        }

        private IEnumerator Tick()
        {
            while (!_isGameOver)
            {
                yield return new WaitForSeconds(TickSpeed);
                _snake.Tick(Input.Instance.CurrentDirection);
            }
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
        }

        private Vector3 GetRandomUnoccupiedPosition()
        {
            // Find random unoccupied position.
            var unoccupiedPositions = _boardPositions.FindAll(p => p.z == 0f);
            var randomIndex = UnityEngine.Random.Range(0, unoccupiedPositions.Count);

            // Occupy the position.
            _boardPositions[_boardPositions.IndexOf(unoccupiedPositions[randomIndex])] = new Vector3(unoccupiedPositions[randomIndex].x, unoccupiedPositions[randomIndex].y, 1f);

            return new Vector3(unoccupiedPositions[randomIndex].x, 0f, unoccupiedPositions[randomIndex].y);
        }
    }
}