using System;
using UnityEngine;

namespace BallForever
{
    public class Wall : MonoBehaviour
    {
        public static event Action OnBallHit;
        public static event Action OnBallPassed;
        public event Action<Wall> OnInvise;
        
        [SerializeField] private Catcher _catcher;

        private float _speed = 0;

        private void Start()
        {
            _catcher.Passed += Passed;
        }

        private void Update()
        {
            transform.position += new Vector3(_speed, 0, 0) * Time.deltaTime;
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        private void Passed()
        {
            OnBallPassed?.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            OnBallHit?.Invoke();
        }

        private void OnBecameInvisible()
        {
            OnInvise?.Invoke(this);
        }
    }
}
