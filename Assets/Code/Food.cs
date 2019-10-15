using System;
using UnityEngine;

namespace Code
{
    public class Food
    {
        public Vector2 Position;
        public Guid Guid = Guid.NewGuid();
        public bool IsEaten = false;

        public Food(Vector2 position)
        {
            Position = position;
        }
    }
}