using UnityEngine;

namespace Code
{
    public class TileView : MonoBehaviour
    {
        [SerializeField] private Sprite tileSprite;
        private SpriteRenderer _spriteRenderer;

        public void Init(Tile tile)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = tileSprite;
            transform.position = tile.Position;
        }
    }
}