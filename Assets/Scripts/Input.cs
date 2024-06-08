using UnityEngine;

namespace SnakeGame
{

    public class Input : MonoBehaviour
    {
        public static Input Instance { get; private set; }

        void Awake()
        {
            CurrentDirection = MovementDirection.None;

            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public MovementDirection CurrentDirection { get; private set; }

        public enum MovementDirection
        {
            None,
            Up,
            Down,
            Left,
            Right
        }

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow) && CurrentDirection != MovementDirection.Down)
            {
                CurrentDirection = MovementDirection.Up;
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow) && CurrentDirection != MovementDirection.Up)
            {
                CurrentDirection = MovementDirection.Down;
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow) && CurrentDirection != MovementDirection.Right)
            {
                CurrentDirection = MovementDirection.Left;
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow) && CurrentDirection != MovementDirection.Left)
            {
                CurrentDirection = MovementDirection.Right;
            }
        }
    }
}
