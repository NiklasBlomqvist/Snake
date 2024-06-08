using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SnakeGame
{
    public class Snake : MonoBehaviour
    {

        public Action EatTreat;

        [SerializeField]
        private GameObject tailPrefab;


        private Vector3 nextTailPosition;

        private float _movementPerTick;

        private List<Tail> _tails = new List<Tail>();

 
        public void Initialize(float movementPerTick)
        {
            _movementPerTick = movementPerTick;
        }

        public void Tick(Input.MovementDirection currentDirection)
        {
            Move(currentDirection);

            foreach (var tail in _tails)
            {
                tail.Tick();
            }
        }

        private void Move(Input.MovementDirection currentDirection)
        {
            switch (currentDirection)
            {
                case Input.MovementDirection.Up:
                    transform.position += Vector3.forward * _movementPerTick;
                    nextTailPosition = transform.position + Vector3.back * _movementPerTick;
                    break;
                case Input.MovementDirection.Down:
                    transform.position += Vector3.back * _movementPerTick;
                    nextTailPosition = transform.position + Vector3.forward * _movementPerTick;
                    break;
                case Input.MovementDirection.Left:
                    transform.position += Vector3.left * _movementPerTick;
                    nextTailPosition = transform.position + Vector3.right * _movementPerTick;
                    break;
                case Input.MovementDirection.Right:
                    transform.position += Vector3.right * _movementPerTick;
                    nextTailPosition = transform.position + Vector3.left * _movementPerTick;
                    break;
                case Input.MovementDirection.None:
                    break;
            }
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            var treat = other.GetComponentInParent<Treat>();
            if (treat == null) return;
            Eat(treat);
        }

        private void Eat(Treat treat)
        {
            treat.GetEaten();

            Grow();

            EatTreat?.Invoke();
        }

        private void Grow()
        {
            var tail = Instantiate(tailPrefab, new Vector3(nextTailPosition.x, tailPrefab.transform.position.y, nextTailPosition.z), Quaternion.identity).GetComponent<Tail>();


            // If there are no tails, make the tail follow the head. Otherwise, make the tail follow the last tail.
            if (_tails.Count < 1)
            {
                tail.Follow(transform);
            }
            else
            {
                tail.Follow(_tails[_tails.Count - 1].transform);
            }

            _tails.Add(tail);
        }

        internal bool IsNextMoveValid(SnakeGame.Input.MovementDirection nextMoveDirection)
        {
            throw new NotImplementedException();
        }
    }
}