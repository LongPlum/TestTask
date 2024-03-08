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
    [SerializeField] private GameObject _leftBackGroundPrefab;
    [SerializeField] private GameObject _rightBackGroundPrefab;
    [SerializeField] private Transform _backGroundGO;
    [SerializeField] private RectTransform _leftBackGroundPosition;
    [SerializeField] private RectTransform _rightBackGroundPosition;


    private GameObject _currentGameObject;
    private ObstaclePoolItem[] _obstacleEnumValues;
    private float _timer;
    private bool _isSpawnDelayed;
    private bool _isSpawnAllowed;


    private ObstaclePoolItem ItemToSpawn => _obstacleEnumValues[UnityEngine.Random.Range(0, _obstaclePool.GetPoolItemLength)];

    void Start()
    {
        _levelmanager.GameStarted += StartSpawn;
        _playerCollision.GameOver += StopSpawn;
        _playerCollision.GameOver += StopAllObstacles;

        _obstacleEnumValues = (ObstaclePoolItem[])Enum.GetValues(typeof(ObstaclePoolItem));
    }

    private void Update()
    {
        if (_isSpawnDelayed && _isSpawnAllowed)
        {
            _timer += Time.deltaTime;
            if (_timer >= _spawnTimer)
            {
                _currentGameObject = _obstaclePool.TakeObstacle(ItemToSpawn);
                _obstaclePool.GetDirMoveSpeedComponent(_currentGameObject).ObstacleMoveSpeed = _obstacleStartMoveSpeed + _levelmanager.Acceleration;
                _currentGameObject.transform.position = new Vector3(UnityEngine.Random.Range(-1.5f, 1.5f), 0, UnityEngine.Random.Range(25, 30));

                _timer = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider Other)
    {
        if (Other.transform.parent.TryGetComponent<Obstacle>(out Obstacle Obs))
        {
            _obstaclePool.ReleaseObstacle(Obs.gameObject);
        }
        else if (Other.transform.parent.TryGetComponent<BackGround>(out BackGround BackGround))
        {
            Destroy(Other.gameObject);
            SpawnBackGround();
        }
    }

    private void StartSpawn()
    {
        _isSpawnAllowed = true;
        StartCoroutine(DelaySpawn(_spawnDelayTime));
    }

    private void StopSpawn()
    {
        _isSpawnAllowed = false;
    }

    private void SpawnBackGround()
    {
       var Go = Instantiate(_leftBackGroundPrefab);
        Go.transform.position = _leftBackGroundPosition.position;

        var Go2 = Instantiate(_rightBackGroundPrefab);
        Go2.transform.position = _rightBackGroundPosition.position;
    }

   

    private void StopAllObstacles()
    {
        foreach (var obstacle in _obstaclePool.ObstacleOnSceneDirMove)
        {
            obstacle.ObstacleMoveSpeed = 0;
        }
    }   
    
    private IEnumerator DelaySpawn(float SpawnDelayTime)
    {
        yield return new WaitForSeconds(SpawnDelayTime);
        _isSpawnDelayed = true;
    }
}
