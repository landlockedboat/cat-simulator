using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Code
{
    public class GameView : Singleton<GameView>
    {
        private int _width = 50, _height = 50;
        private int _startingCats = 50;
        private List<List<Tile>> _tiles;

        [FormerlySerializedAs("_tilePrefab")] [SerializeField]
        private GameObject tilePrefab;

        [FormerlySerializedAs("_catPrefab")] [SerializeField]
        private GameObject catPrefab;

        [SerializeField] private float foodSpawnRate = 1f;

        [SerializeField] private GameObject foodPrefab;
        private Dictionary<Guid, Cat> _cats;
        private Dictionary<Guid, Food> _food;

        private void Awake()
        {
            Random.InitState(123);
            _cats = new Dictionary<Guid, Cat>();
        }

        // Start is called before the first frame update
        private void Start()
        {
            // Init tiles
            _tiles = new List<List<Tile>>(_width);
            for (var i = 0; i < _width; i++)
            {
                _tiles.Add(new List<Tile>(_height));
                for (var j = 0; j < _height; j++)
                {
                    // 1 means walkable I guess
                    var tileView = Instantiate(tilePrefab, transform).GetComponent<TileView>();
                    _tiles[i].Add(new Tile(new Vector2(i, j), 1));
                    tileView.Init(_tiles[i][j]);
                }
            }

            // Init cats
            for (int i = 0; i < _startingCats; i++)
            {
                var catView = Instantiate(catPrefab, transform).GetComponent<CatView>();
                var newCat = new Cat(GetRandomMapPosition(), this);
                _cats.Add(newCat.Guid, newCat);
                catView.Init(newCat);
            }

            _food = new Dictionary<Guid, Food>();
            StartCoroutine(SpawnFood());
        }

        private IEnumerator SpawnFood()
        {
            while (true)
            {
                var foodView = Instantiate(foodPrefab).GetComponent<FoodView>();
                var food = new Food(GetRandomMapPosition());
                _food.Add(food.Guid, food);
                foodView.Init(food);
                yield return new WaitForSeconds(foodSpawnRate);
            }
        }

        private void Update()
        {
            foreach (var entry in _cats)
            {
                entry.Value.Tick(Time.deltaTime);
            }

            if (_cats.Any(kvp => kvp.Value.IsDead))
            {
                _cats = _cats.Where(kvp => !kvp.Value.IsDead).ToDictionary(c => c.Key, c => c.Value);
            }
            
            if (_food.Any(kvp => kvp.Value.IsEaten))
            {
                _food = _food.Where(kvp => !kvp.Value.IsEaten).ToDictionary(c => c.Key, c => c.Value);
            }
        }

        public Vector2 GetRandomMapPosition()
        {
            return new Vector2(Random.Range(0, _width), Random.Range(0, _height));
        }

        public Food GetNearbyFood(Vector2 position, float sensoryRange)
        {
            foreach (var entry in _food)
            {
                var food = entry.Value;
                if (Vector2.Distance(position, food.Position) < sensoryRange)
                {
                    return food;
                }
            }

            return null;
        }

        public void DeleteFood(Guid guid)
        {
            _food[guid].IsEaten = true;
        }

        public void DeleteCat(Guid guid)
        {
            _cats[guid].IsDead = true;
        }

        public int GetCatPopulation()
        {
            return _cats.Count;
        }
    }
}