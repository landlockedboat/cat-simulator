using System;
using UnityEditor;
using UnityEngine;

namespace Code
{
    public class Cat
    {
        public Guid Guid = Guid.NewGuid();

        private static float _closeEnoughDistance = .1f;

        public Vector2 Position;
        private Food _targetFood;
        private Vector2 _desiredPosition;
        private float _speed = 5;
        private GameView _gameView;

        private float _sensoryRange = 3f;
        private float _hungerMultiplier = .1f;

        private float _hunger = 0f;
        private float _health = 1f;
        private float _urgeToEat = 0f;

        private CatState _state;
        public bool IsDead = false;

        private enum CatState
        {
            LookingForFood,
            GoingToEatFood,
            Resting
        }

        public Cat(Vector2 position, GameView gameView)
        {
            Position = position;
            _gameView = gameView;
            _state = CatState.Resting;
            _desiredPosition = _gameView.GetRandomMapPosition();
        }

        public void Tick(float deltaTime)
        {
            switch (_state)
            {
                case CatState.LookingForFood:
                    LookForFood(deltaTime);
                    GetHungrier(deltaTime);
                    break;
                case CatState.Resting:
                    GetHungrier(deltaTime);
                    _urgeToEat = _hunger;
                    if (_urgeToEat > .6f)
                    {
                        _state = CatState.LookingForFood;
                    }

                    break;
                case CatState.GoingToEatFood:
                    if (_targetFood.IsEaten)
                    {
                        _state = CatState.Resting;
                    }
                    
                    if (Vector2.Distance(Position, _desiredPosition) < _closeEnoughDistance)
                    {
                        Debug.Log("Ate Food!");
                        // monch!
                        _hunger = 0f;
                        _urgeToEat = 0f;
                        _state = CatState.Resting;
                        _gameView.DeleteFood(_targetFood.Guid);
                    }
                    Position = Vector2.MoveTowards(Position, _desiredPosition, _speed * deltaTime);
                    GetHungrier(deltaTime);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void GetHungrier(float deltaTime)
        {
            _hunger += deltaTime * _hungerMultiplier;

            if (!(_hunger > 1))
            {
                return;
            }

            _health -= deltaTime * _hungerMultiplier;
            if (_health < 0)
            {
                _gameView.DeleteCat(Guid);
            }
        }

        private void LookForFood(float deltaTime)
        {
            var nearbyFood = _gameView.GetNearbyFood(Position, _sensoryRange);
            if (nearbyFood != null)
            {
                Debug.Log("Found Food!");
                _state = CatState.GoingToEatFood;
                _desiredPosition = nearbyFood.Position;
                _targetFood = nearbyFood;
                return;
            }

            if (Vector2.Distance(Position, _desiredPosition) < _closeEnoughDistance)
            {
                _desiredPosition = _gameView.GetRandomMapPosition();
            }

            Position = Vector2.MoveTowards(Position, _desiredPosition, _speed * deltaTime);
        }
    }
}