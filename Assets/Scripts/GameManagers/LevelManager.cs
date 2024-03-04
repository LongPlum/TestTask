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
    [SerializeField] private AdManager _adManager;
    [SerializeField] private GameObject _adCanvas;





    public float GameTime { get; private set; }
    public float Acceleration { get; private set; }
    public float Score { get; private set; }


    private float _accelerationTimeCounter;
    private bool _isLevelBegin;
    private bool _isGameWasStarted;
    private bool _isGamePaused;
    public Action GameStarted;


    private IEnumerator EnableAdCanvas()
    {
        yield return new WaitForSeconds(2);
        _adCanvas.SetActive(true);
    }

    private void RewardPlayer()
    {
        Score *= 2;
    }
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
        StartCoroutine(EnableAdCanvas());
        _playerCollision.GameOver += StopLevel;
        _pauseManager.GamePaused += PauseGame;
        _pauseManager.GameResumed += ResumeGame;
        _adManager.RewardPlayer += RewardPlayer;
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
        if (Input.touchCount == 1 && !_isLevelBegin)
        {
            GameStarted.Invoke();
            _playerAnimation.StartRunning();
            _isLevelBegin = true;
            _isGameWasStarted = false;

        }


        if (_isLevelBegin && !_isGamePaused)
        {

            GameTime += Time.deltaTime;
            if (Math.Round(GameTime % 1) == 0)
            {
                Score += 1;
            }

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
