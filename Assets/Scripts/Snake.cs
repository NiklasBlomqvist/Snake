using System;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame
{
    public class Snake : MonoBehaviour
    {

        [SerializeField]
        private GameObject tailPrefab;


        public Action EatTreat;


        private float _movementPerTick;
        private List<Tail> _tails = new List<Tail>();


        public void Initialize(float movementPerTick)
        {
            _movementPerTick = movementPerTick;
        }

        public void Tick(Vector3 nextDirection)
        {
            Move(nextDirection);

            foreach (var tail in _tails)
            {
                tail.Tick();
            }
        }

        private void Move(Vector3 nextDirection)
        {
            transform.position += nextDirection * _movementPerTick;
            transform.LookAt(nextDirection + transform.position);
        }

        private void OnTriggerEnter(Collider other)
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
            var tail = Instantiate(tailPrefab).GetComponent<Tail>();
            
            // If there are no tails, make the tail follow the head. Otherwise, make the tail follow the last tail.
            if (_tails.Count < 1)
            {
                tail.transform.position = transform.position;
                tail.Follow(transform);
            }
            else
            {
                tail.transform.position = _tails[_tails.Count - 1].transform.position;
                tail.Follow(_tails[_tails.Count - 1].transform);
            }

            _tails.Add(tail);

        }

        public Vector3 GetHeadPosition()
        {
            return transform.position;
        }

        public List<Vector3> GetTailPositions()
        {
            var tailPositions = new List<Vector3>();

            foreach (var tail in _tails)
            {
                tailPositions.Add(tail.transform.position);
            }

            return tailPositions;
        }
    }
}