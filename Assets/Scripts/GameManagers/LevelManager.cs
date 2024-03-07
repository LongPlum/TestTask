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
    [SerializeField] private GameObject _startButton;



    private float _accelerationTimeCounter;
    private bool _isGamePaused;

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
        _pauseManager.GamePaused += PauseGame;
        _pauseManager.GameResumed += ResumeGame;
    }

    public void StartGameClick()
    {
        GameStarted.Invoke();
        _playerAnimation.StartRunning();
        IsLevelBegin = true;
        _startButton.SetActive(false);
    }

    void Update()
    {

        if (IsLevelBegin && !_isGamePaused)
        {
            GameTime += Time.deltaTime;
            Score += Mathf.CeilToInt(Time.deltaTime);


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
