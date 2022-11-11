using System;
using UnityEngine;

namespace BallForever
{
    public class FoneController : MonoBehaviour
    {
        [SerializeField] private Material _foneMaterial;
        private float _speed;
        private float _offset;

        private void Update()
        {
            _offset += _speed * Time.deltaTime;
            _foneMaterial.SetFloat("_offset", _offset);
        }

        public void SetSpeed(float speed)
        {
            _speed = speed/50;
        }
    }
}
