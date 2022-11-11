using System;
using UnityEngine;

namespace BallForever
{
    public class BallController : MonoBehaviour
    {
        [SerializeField]private float _verticalForce = 0;
        [SerializeField]private float _userForce = 0;
        private float _resultForce;

        public static event Action OnBallInvise;
        
        private void Update()
        {
            _resultForce = _verticalForce;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                _resultForce = _userForce;
            }
            
            Fall();
        }

        public void SetForce(float force)
        {
            _verticalForce = -force;
            _userForce = force;
        }

        public void SetPosition(Vector3 pos)
        {
            transform.position = pos;
        }

        private void Fall()
        {
            transform.position += new Vector3(0, _resultForce, 0) * Time.deltaTime;
        }

        private void OnBecameInvisible()
        {
            OnBallInvise?.Invoke();
        }
    }
}

