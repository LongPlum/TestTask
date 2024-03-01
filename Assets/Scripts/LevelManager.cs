using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    
    [SerializeField] private ObstaclePool _obstaclePool;

    public float GameTime { get; private set; }
    public float Acceleration { get; private set; }

    private float _accelerationTimeCounter;

    void Update()
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
