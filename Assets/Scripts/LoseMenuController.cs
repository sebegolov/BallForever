using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BallForever
{
    public class LoseMenuController : MenuController
    {
        [SerializeField] private TextMeshProUGUI _textDifficult;
        [SerializeField] private TextMeshProUGUI _textResult;
        [SerializeField] private TextMeshProUGUI _textBestResult;
        [SerializeField] private TextMeshProUGUI _textCountGame;

        public void SetLoseParameters(DifficultGame difficultGame, int result, int bestResult, int countGame)
        {
            _textDifficult.text = GetStringDifficult(difficultGame);
            _textResult.text = result.ToString();
            _textBestResult.text = bestResult.ToString();
            _textCountGame.text = countGame.ToString();
        }

        private string GetStringDifficult(DifficultGame difficultGame)
        {
            switch (difficultGame)
            {
                case DifficultGame.Low: return "Низкая";
                case DifficultGame.Medium: return "Средняя";
            }

            return "Высокая";
        }
    }
}
