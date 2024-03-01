using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float _spawnTimer = 0.5f;
    [SerializeField] private float _spawnDelayTime = 4f;
    [SerializeField] private float _obstacleStartMoveSpeed = 10f;
    [SerializeField] private ObstaclePool _obstaclePool;
    [SerializeField] private PlayerCollision _playerCollision;
    [SerializeField] private LevelManager _levelmanager;


    private GameObject _currentGameObject;
    private _obstaclePoolItem[] _obstacleEnumValues;
    private float _timer;
    private bool _flagDelay = false;


    private _obstaclePoolItem ItemToSpawn => _obstacleEnumValues[UnityEngine.Random.Range(0, _obstaclePool.GetPoolItemLength)];

    void Start()
    {
        _playerCollision.OnPlayerCollision += SpawnStop;
        _obstacleEnumValues = (_obstaclePoolItem[])Enum.GetValues(typeof(_obstaclePoolItem));
        StartCoroutine(DelaySpawn(_spawnDelayTime));
    }

    private void Update()
    {
        if (_flagDelay)
        {
            _timer += Time.deltaTime;
            if (_timer >= _spawnTimer)
            {
                _currentGameObject = _obstaclePool.TakeObstacle(ItemToSpawn);
                _currentGameObject.GetComponent<DirectionalMovement>().ObstacleMoveSpeed = _obstacleStartMoveSpeed + _levelmanager.Acceleration;
                _currentGameObject.transform.position = new Vector3(UnityEngine.Random.Range(-1.5f, 1.5f), 0, UnityEngine.Random.Range(25, 30));

                _timer = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.TryGetComponent<Obstacle>(out Obstacle obs))
        {
            _obstaclePool.ReleaseObstacle(obs.gameObject);
        }
    }

    private void SpawnStop(float delay)
    {
        _timer -= delay;
    }

    private IEnumerator DelaySpawn(float _spawnDelayTime)
    {
        yield return new WaitForSeconds(_spawnDelayTime);
        _flagDelay = true;
    }
}
