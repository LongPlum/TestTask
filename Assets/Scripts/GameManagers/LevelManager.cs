using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private float _backGroundStartMS;

    private float _accelerationTimeCounter;
    private bool _isGamePaused;
    private DirectionalMovement _leftDirMove;
    private DirectionalMovement _rightDirMove;


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
    }
    private void StopLevel()
    {
        IsLevelBegin = false;
    }

    void Start()
    {
        _playerCollision.GameOver += StopLevel;
        _playerCollision.GameOver += StopBackGround;
        _pauseManager.GamePaused += PauseGame;
        _pauseManager.GameResumed += ResumeGame;
        _leftDirMove = _leftBackGround.GetComponent<DirectionalMovement>();
        _rightDirMove = _rightBackGround.GetComponent<DirectionalMovement>();
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

    private void StopBackGround()
    {
        _leftDirMove.MoveSpeed = 0;
        _rightDirMove.MoveSpeed = 0;
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
                Acceleration += 0.05f;
                
                _accelerationTimeCounter = 0;
            }
            if (_obstaclePool.ObstacleOnSceneDirMove.Count > 0)
            {
                foreach (var Component in _obstaclePool.ObstacleOnSceneDirMove)
                {
                    Component.MoveSpeed += Acceleration;
                }
            }
            _leftDirMove.MoveSpeed += Acceleration;
            _rightDirMove.MoveSpeed += Acceleration;

        }
    }
}
