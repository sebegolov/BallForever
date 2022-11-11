using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace BallForever
{
    public class WallsController : MonoBehaviour
    {
        [SerializeField] private Wall _wall;
        [SerializeField] private Vector2Int _topHighSpawn;
        [SerializeField] private Vector2Int _botHighSpawn;

        private float _speed;
        private List<Wall> _walls = new List<Wall>();
        private List<Wall> _inviseWalls = new List<Wall>();
        private float _timeSpawn;
        private float _time;
        private bool _variant = true;

        private void Update()
        {
            _time += Time.deltaTime;

            SpawnWall();
        }

        private void SpawnWall()
        {
            if (_time > _timeSpawn )
            {
                _time = 0;
                if (_inviseWalls.Count > 0)
                {
                    _inviseWalls[0].transform.position = GetRandomPositiom();
                    _inviseWalls[0].SetSpeed(_speed);
                    _inviseWalls.RemoveAt(0);
                }
                else
                {
                    Wall newWall = Instantiate(_wall, GetRandomPositiom(), new Quaternion());
                    newWall.SetSpeed(_speed);
                    newWall.OnInvise += WallInvise;
                    _walls.Add(newWall);
                }
            }
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;

            if (_walls.Count > 0)
            {
                foreach (var wall in _walls)
                {
                    wall.SetSpeed(speed);
                }
            }
        }

        public void SetTimeSpawn(float time)
        {
            _timeSpawn = time;
        }

        private Vector3 GetRandomPositiom()
        {
            Random random = GameController.Random;
            float position = 0;
            _variant = !_variant;

            switch (_variant)
            {
                case true: position = random.Next(_botHighSpawn.x, _botHighSpawn.y); break;
                case false: position = random.Next(_topHighSpawn.x, _topHighSpawn.y); break;
            }
            

            return new Vector3(10,position,0);
        }

        private void WallInvise(Wall wall)
        {
            _inviseWalls.Add(wall);
        }

        public void Reset()
        {
            _inviseWalls.Clear();
            
            foreach (var wall in _walls)
            {
                wall.OnInvise -= WallInvise;
                Destroy(wall.gameObject);
            }
            _walls.Clear();
        }
    }
}
