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
    [SerializeField] private PlayerAnimation _playerAnimation;

    public event Action GameOver;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.TryGetComponent<Obstacle>(out Obstacle obs))
        {
            _obstaclePool.ReleaseObstacle(obs.gameObject);
            GameOver.Invoke();
            _playerAnimation.PlayerDeath();
            _gameOverMenu.SetActive(true);
        }
    
    }
    
}
