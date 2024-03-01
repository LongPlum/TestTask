using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private ObstaclePool _obstaclePool;
    [SerializeField] private PlayerAnimation _playerAnimation;


    public float GameTime { get; private set; }
    public float Acceleration { get; private set; }

    private float _accelerationTimeCounter;
    private bool _isLevelBegin;
    private Touch _touchInput;

    public Action GameStarted;

    void Update()
    {


#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.Space) && !_isLevelBegin)
        {
            GameStarted.Invoke();
            _playerAnimation.StartRunning();
            _isLevelBegin = true;
        }
#else
        
        if (Input.touchCount > 0)
        {
            _touchInput = Input.GetTouch(0);
        }

        if (_touchInput.phase == TouchPhase.Stationary && !_isLevelBegin)
        {
            GameStarted.Invoke();
            _playerAnimation.StartRunning();
            _isLevelBegin = true;
        }
#endif


        if (_isLevelBegin)
        {

            GameTime += Time.deltaTime;
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
