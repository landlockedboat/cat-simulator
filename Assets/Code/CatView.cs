using UnityEngine;

namespace Code
{
    public class CatView : MonoBehaviour
    {
        private Cat _cat;
        public void Init(Cat cat)
        {
            _cat = cat;
        }

        private void Update()
        {
            if (_cat.IsDead)
            {
                Destroy(gameObject);
            }
            transform.position = _cat.Position;
        }
    }
}