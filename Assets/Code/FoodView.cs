using UnityEngine;

namespace Code
{
    public class FoodView : MonoBehaviour
    {
        private Food _food;
        
        public void Init(Food food)
        {
            _food = food;
            transform.position = food.Position;
        }

        public void Update()
        {
            if (_food.IsEaten)
            {
                Destroy(gameObject);
            }
        }
    }
}