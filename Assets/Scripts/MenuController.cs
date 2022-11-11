using System;
using TMPro;
using UnityEngine;

namespace BallForever
{
    public class MenuController : MonoBehaviour
    {
        public event Action<DifficultGame> OnStartGame; 
        
        [SerializeField] protected TMP_Dropdown _dropdown;

        protected DifficultGame _difficult = DifficultGame.Medium;

        protected void Awake()
        {
            _dropdown.onValueChanged.AddListener(SetDifficult);
        }

        public void StartGame()
        {
            OnStartGame?.Invoke(_difficult);
        }

        protected void SetDifficult(int i)
        {
            switch (i)
            {
                case 0: _difficult = DifficultGame.Medium; break;
                case 1: _difficult = DifficultGame.High; break;
                case 2: _difficult = DifficultGame.Low; break;
            }
        }

    }
}
