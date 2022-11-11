using System;
using UnityEngine;

namespace BallForever
{
    public class Catcher : MonoBehaviour
    {
        public event Action Passed;
        private void OnTriggerExit(Collider other)
        {
            Passed?.Invoke();
        }
    }
}
