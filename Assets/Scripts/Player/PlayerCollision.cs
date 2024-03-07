using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private ObstaclePool _obstaclePool;
    [SerializeField] private float _knockBackDuration;
    [SerializeField] private float _knockBackDistance;
    [SerializeField] private GameObject _gameOverMenu;
    [SerializeField] private PlayerAnimation _playerAnimation;
    [SerializeField] private GameObject _player;


    public event Action GameOver;

   
    private void OnTriggerEnter(Collider Other)
    {
        if (Other.transform.parent.TryGetComponent<Obstacle>(out Obstacle obs))
        {
            GameOver.Invoke();
            _obstaclePool.ReleaseObstacle(obs.gameObject);
            _playerAnimation.PlayerDeath();
            var Pos = _player.transform.position;
            Pos.y += 0.1f;
            _player.transform.position = Pos;
            _gameOverMenu.SetActive(true);
            FireBaseManager.FireBaseManagerInstance.SafeScore();
        }
    
    }
    
}
