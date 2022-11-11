using System;
using TMPro;
using UnityEngine;

namespace BallForever
{
    public class UIController : MonoBehaviour
    {
        public event Action<DifficultGame> OnStart; 
        
        [SerializeField] private TextMeshProUGUI _pointText;
        [SerializeField] private StartMenuController _startMenuController;
        [SerializeField] private LoseMenuController _loseMenuController;
        [SerializeField] private GameObject _gameMenu;

        private void Awake()
        {
            _startMenuController.OnStartGame += StartGame;
            _loseMenuController.OnStartGame += StartGame;
        }

        private void Start()
        {
            ActiveStartMenu(true);
        }

        private void StartGame(DifficultGame difficult)
        {
            OnStart?.Invoke(difficult);
            ActiveStartMenu(false);
            ActiveLoseMenu(false);
        }

        private void ActiveStartMenu(bool active)
        {
            _startMenuController.gameObject.SetActive(active);
            _gameMenu.SetActive(!active);
        }
        
        private void ActiveLoseMenu(bool active)
        {
            _loseMenuController.gameObject.SetActive(active);
            _gameMenu.SetActive(!active);
        }

        public void SetPoint(int point)
        {
            _pointText.text = point.ToString();
        }

        public void Lose(DifficultGame difficultGame, int result, int bestResult, int countGame)
        {
            ActiveLoseMenu(true);
            _loseMenuController.SetLoseParameters(difficultGame, result, bestResult, countGame);
        }
    }
}