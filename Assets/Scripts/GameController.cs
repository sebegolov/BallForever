using System;
using TMPro;
using UnityEngine;
using Random = System.Random;

namespace BallForever
{
    public class GameController : MonoBehaviour
    {
        public static Random Random = new Random();

        [Header("Start settings")]
        [SerializeField] private Settings _lowSettings;

        [SerializeField] private Settings _mediumSettings;
        [SerializeField] private Settings _hardSettings;


        [Header("Controllers")]
        [SerializeField] private BallController _ballController;

        [SerializeField] private WallsController _wallsController;
        [SerializeField] private FoneController _foneController;
        [SerializeField] private UIController _uiController;

        private int _points = 0;
        private float _time = 0;

        private float _speed = 0;
        private float _increaseSpeed = 0;
        private float _timeIncrease = float.MaxValue;
        private float _timeWallSpawn = int.MaxValue;
        private float _ballFallForce = 0;
        private DifficultGame _difficult = DifficultGame.Medium;

        private GameHistory _lowDifficultHistory;
        private GameHistory _mediumDifficultHistory;
        private GameHistory _HardDifficultHistory;

        private bool _gameProcess = false;

        private string _lowDifHisBestResult = "LowDifficult_BestResult";
        private string _lowDifHisCountGame  = "LowDifficult_CountGame";
        private string _medDifHisBestResult = "MediumDifficult_BestResult";
        private string _medDifHisCountGame  = "MediumDifficult_CountGame";
        private string _harDifHisBestResult = "HardDifficult_BestResult";
        private string _harDifHisCountGame  = "HardDifficult_CountGame";
        
        private void Awake()
        {
            LoadHistory();
            _ballController = Instantiate(_ballController);
            
            _uiController.OnStart += StartGame;
            BallController.OnBallInvise += LoseGame;
            Wall.OnBallHit += LoseGame;
            Wall.OnBallPassed += AddPoint;
            
            SetControllersSettings();
        }

        private void Update()
        {
            if (_gameProcess)
            {
                _time += Time.deltaTime;
            }

            if (_time > _timeIncrease)
            {
                _time = 0;
                _speed *= _increaseSpeed;
                _timeWallSpawn /= _increaseSpeed;
                _ballFallForce *= _increaseSpeed;
                SetControllersSettings();
            }
        }

        private void LoadHistory()
        {
            _lowDifficultHistory = new GameHistory();
            _mediumDifficultHistory = new GameHistory();
            _HardDifficultHistory = new GameHistory();

            if (PlayerPrefs.HasKey(_lowDifHisBestResult)) { _lowDifficultHistory.bestResult = PlayerPrefs.GetInt(_lowDifHisBestResult); }
            if (PlayerPrefs.HasKey(_lowDifHisCountGame)) { _lowDifficultHistory.countGame = PlayerPrefs.GetInt(_lowDifHisCountGame); }
            if (PlayerPrefs.HasKey(_medDifHisBestResult)) { _mediumDifficultHistory.bestResult = PlayerPrefs.GetInt(_medDifHisBestResult); }
            if (PlayerPrefs.HasKey(_medDifHisCountGame)) { _mediumDifficultHistory.countGame = PlayerPrefs.GetInt(_medDifHisCountGame); }
            if (PlayerPrefs.HasKey(_harDifHisBestResult)) { _HardDifficultHistory.bestResult = PlayerPrefs.GetInt(_harDifHisBestResult); }
            if (PlayerPrefs.HasKey(_harDifHisCountGame)) { _HardDifficultHistory.countGame = PlayerPrefs.GetInt(_harDifHisCountGame); }
        }

        private void SaveHistory()
        {
            PlayerPrefs.SetInt(_lowDifHisBestResult, _lowDifficultHistory.bestResult);
            PlayerPrefs.SetInt(_lowDifHisCountGame, _lowDifficultHistory.countGame);
            PlayerPrefs.SetInt(_medDifHisBestResult , _mediumDifficultHistory.bestResult);
            PlayerPrefs.SetInt(_medDifHisCountGame, _mediumDifficultHistory.countGame);
            PlayerPrefs.SetInt(_harDifHisBestResult , _HardDifficultHistory.bestResult);
            PlayerPrefs.SetInt(_harDifHisCountGame, _HardDifficultHistory.countGame);
                
            PlayerPrefs.Save();
        }

        private void SetControllersSettings()
        {
            _wallsController.SetTimeSpawn(_timeWallSpawn);
            _wallsController.SetSpeed(_speed);
            
            _foneController.SetSpeed(_speed);
            
            _ballController.SetForce(_ballFallForce);
        }

        private void ResetBallPosition()
        {
            _ballController.transform.position = new Vector3();
        }

        private void StartGame(DifficultGame difficult)
        {
            _difficult = difficult;
            SetDifficult(difficult);
            _gameProcess = true;
            SetControllersSettings();
            ResetBallPosition();
        }

        private void SetDifficult(DifficultGame difficult)
        {
            switch (difficult)
            {
                case DifficultGame.Low: SetSettings(_lowSettings); break;
                case DifficultGame.Medium: SetSettings(_mediumSettings); break;
                case DifficultGame.High: SetSettings(_hardSettings); break;
            }
        }

        private void SetSettings(Settings settings)
        {
            _speed = settings.speed;
            _increaseSpeed = settings.increaseSpeed;
            _timeIncrease = settings.timeIncrease ;
            _timeWallSpawn = settings.timeWallSpawn;
            _ballFallForce =settings.ballFallForce;
        }

        private void LoseGame()
        {
            SaveResult();
            GameHistory history = GetHistory();
            _time = 0;
            _gameProcess = false;
            SetSettings(new Settings(0));
            SetControllersSettings();
            _uiController.Lose(_difficult, _points, history.bestResult, history.countGame);
            _uiController.SetPoint(0);
            _wallsController.Reset();
            _points = 0;
        }

        private void SaveResult()
        {
            GameHistory gameHistory = GetHistory();
            gameHistory.AddGame(_points);
            SaveHistory();
        }

        private GameHistory GetHistory()
        {
            switch (_difficult)
            {
                case DifficultGame.Low: return _lowDifficultHistory;
                case DifficultGame.Medium: return _mediumDifficultHistory;
            }

            return _HardDifficultHistory;
        }

        private void AddPoint()
        {
            _points++;
            _uiController.SetPoint(_points);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }

    [Serializable]
    public class GameHistory
    {
        public DifficultGame _difficult;
        public int bestResult;
        public int countGame;

        public void AddGame(int result)
        {
            countGame++;
            bestResult = result > bestResult ? result : bestResult;
        }
    }

    [Serializable]
    public struct Settings
    {
        public float speed;                //скорость
        public float increaseSpeed;        //увеличитель скорости
        public float timeIncrease;        //время через которое надо увеличивать
        public float timeWallSpawn;        //промежуток между спавном стенок
        public float ballFallForce;        //скорость падения мяча

        public Settings(float s)
        {
            speed = s;
            increaseSpeed = 0;
            timeIncrease = float.MaxValue;
            timeWallSpawn = float.MaxValue;
            ballFallForce = 0;
        }
    }
    
    public enum DifficultGame
    {
        Low, Medium, High
    }

}
