using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private ObstaclePool _obstaclePool;
    [SerializeField] private PlayerAnimation _playerAnimation;
    [SerializeField] private PlayerCollision _playerCollision;


    public float GameTime { get; private set; }
    public float Acceleration { get; private set; }
    public float Score { get; private set; }


    private float _accelerationTimeCounter;
    private bool _isLevelBegin;
    private bool _isGameWasStarted;
    public Action GameStarted;

    private void StopLevel()
    {
        _isLevelBegin = false;
    }

    void Start()
    {
        _playerCollision.GameOver += StopLevel;
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


        if (_isLevelBegin)
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
