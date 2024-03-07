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


    private float _accelerationTimeCounter;
    private bool _isLevelBegin;
    private bool _isGameWasStarted;
    private bool _isGamePaused;

    public float GameTime { get; private set; }
    public float Acceleration { get; private set; }
    public float Score { get; set; }

    
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
        _isLevelBegin = false;
    }

    void Start()
    {
        _playerCollision.GameOver += StopLevel;
        _pauseManager.GamePaused += PauseGame;
        _pauseManager.GameResumed += ResumeGame;
        _isGameWasStarted = true;
    }

    void Update()
    {


#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.Space) && !_isLevelBegin && _isGameWasStarted)
        {
            GameStarted.Invoke();
            _playerAnimation.StartRunning();
            _isLevelBegin = true;
            _isGameWasStarted = false;

        }
#endif
        if (Input.touchCount == 1 && !_isLevelBegin && _isGameWasStarted)
        {
            GameStarted.Invoke();
            _playerAnimation.StartRunning();
            _isLevelBegin = true;
            _isGameWasStarted = false;
        }


        if (_isLevelBegin && !_isGamePaused)
        {
            Score += Time.deltaTime;
            
            _accelerationTimeCounter += Time.deltaTime;

            if (_accelerationTimeCounter >= 5)
            {
                Acceleration += 0.1f;
                _accelerationTimeCounter = 0;
            }

            if (_obstaclePool.ObstacleOnSceneDirMove.Count > 0)
            {
                foreach (var Component in _obstaclePool.ObstacleOnSceneDirMove)
                {
                    Component.ObstacleMoveSpeed += Acceleration;
                }
            }
        }



    }
}
