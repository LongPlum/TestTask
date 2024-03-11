using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private ObstaclePool _obstaclePool;
    [SerializeField] private PlayerAnimation _playerAnimation;
    [SerializeField] private PlayerCollision _playerCollision;
    [SerializeField] private PauseManager _pauseManager;
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private GameObject _startButton;
    [SerializeField] private float _timeToIncreaseMS;
    [SerializeField] private GameObject _leftBackGround;
    [SerializeField] private GameObject _rightBackGround;
    [SerializeField] private Transform _leftSpawnPos;
    [SerializeField] private Transform _rightSpawnPos;
    [SerializeField] private float _backGroundStartMS;

    private float _accelerationTimeCounter;
    private bool _isGamePaused;
    private DirectionalMovement _leftDirMove;
    private DirectionalMovement _rightDirMove;
    private Vector3 _rightPosition;
    private Vector3 _leftPosition;


    public bool IsLevelBegin { get; private set; }
    public float GameTime { get; private set; }
    public float Acceleration { get; private set; }
    public int Score { get; set; }

    
    public event Action GameStarted;

    private void PauseGame()
    {
        _isGamePaused = true;
    }
    private void ResumeGame()
    {
        _isGamePaused = false;
        IsLevelBegin = true;
    }
    private void StopLevel()
    {
        IsLevelBegin = false;
    }

    private void StopBackGround()
    {
        _leftDirMove.MoveSpeed = 0;
        _rightDirMove.MoveSpeed = 0;
    }

    void Start()
    {
        _playerCollision.GameOver += StopLevel;
        _playerCollision.GameOver += StopBackGround;
        _pauseManager.GamePaused += PauseGame;
        _pauseManager.GameResumed += ResumeGame;
        _spawnManager.BackGroundCollision += TranslateBackGroud;
        _leftDirMove = _leftBackGround.GetComponent<DirectionalMovement>();
        _rightDirMove = _rightBackGround.GetComponent<DirectionalMovement>();
        _rightPosition = _rightSpawnPos.transform.position;
        _leftPosition = _leftSpawnPos.transform.position;
    }


    public void StartGameClick()
    {
        GameStarted.Invoke();
        _playerAnimation.StartRunning();
        IsLevelBegin = true;
        _startButton.SetActive(false);
        _leftDirMove.MoveSpeed = _backGroundStartMS;
        _rightDirMove.MoveSpeed = _backGroundStartMS;
    }

  

    private void TranslateBackGroud(GameObject BG)
    {
        if (BG.GetComponent<BackGround>().GetBGType() == BackGroundType.Left_BackGround)
        {
            BG.transform.position = _leftPosition;
        }
        else
         BG.transform.position = _rightPosition;
    }

    void Update()
    {

        if (IsLevelBegin && !_isGamePaused)
        {
            GameTime += Time.deltaTime;
            Score += Mathf.CeilToInt(Time.deltaTime);


            _accelerationTimeCounter += Time.deltaTime;

            if (_accelerationTimeCounter >= _timeToIncreaseMS)
            {
                Acceleration += 0.5f;
                
                _accelerationTimeCounter = 0;

                if (_obstaclePool.ObstacleOnSceneDirMove.Count > 0)
                {
                    foreach (var Component in _obstaclePool.ObstacleOnSceneDirMove)
                    {
                        Component.MoveSpeed = _backGroundStartMS + Acceleration;
                    }
                }
                _leftDirMove.MoveSpeed = _backGroundStartMS + Acceleration;
                _rightDirMove.MoveSpeed = _backGroundStartMS + Acceleration;
            }
        }
    }
}
