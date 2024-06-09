using UnityEngine;

namespace SnakeGame
{

    public class Input : MonoBehaviour
    {
        public static Input Instance { get; private set; }

        public Vector3 NextDirection { get; private set; }


        void Awake()
        {
            // Singleton pattern.
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            NextDirection = Vector3.zero;
        }

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
            {
                NextDirection = Vector3.forward;
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
            {
                NextDirection = Vector3.back;
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
            {
                NextDirection = Vector3.left;
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
            {
                NextDirection = Vector3.right;
            }
        }
    }
}
