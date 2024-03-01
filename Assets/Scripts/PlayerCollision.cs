using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private ObstaclePool _obstaclePool;
    [SerializeField] private float _knockBackDuration;
    [SerializeField] private float _knockBackDistance;
    [SerializeField] private GameObject _gameOverMenu;


    public event Action GameOver;

    private void Start()
    {
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.TryGetComponent<Obstacle>(out Obstacle obs))
        {
            _obstaclePool.ReleaseObstacle(obs.gameObject);
            GameOver.Invoke();
            _gameOverMenu.SetActive(true);
        }
    
    }
    
}
