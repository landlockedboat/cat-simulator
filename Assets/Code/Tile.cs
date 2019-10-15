using UnityEngine;

namespace Code
{
    public class Tile
    {
        public Vector2 Position;
        public int TileType;

        public Tile(Vector2 position, int tileType)
        {
            Position = position;
            TileType = tileType;
        }
    }
}