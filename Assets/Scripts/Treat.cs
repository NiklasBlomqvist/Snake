using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeGame
{
    public class Treat : MonoBehaviour
    {
        public void GetEaten()
        {
            Destroy(gameObject);
        }
    }
}