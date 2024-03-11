using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstaclePool : MonoBehaviour
{
    [SerializeField] private int _poolSize;
    [SerializeField] private List<GameObject> _pooledGameObjects = new();

    private Dictionary<ObstaclePoolItem, Stack<GameObject>> _dictionaryPool = new();
    private Dictionary<ObstaclePoolItem, Func<GameObject>> _factory = new();

    private List<GameObject> _obstacleOnScene = new();
    private List<DirectionalMovement> _obstacleOnSceneDirMove = new();

    public int GetPoolItemLength => _pooledGameObjects.Count;

    public IReadOnlyCollection<GameObject> ObstacleOnScene
    {
        get { return _obstacleOnScene.AsReadOnly(); }
    }
    public IReadOnlyCollection<DirectionalMovement> ObstacleOnSceneDirMove
    {
        get { return _obstacleOnSceneDirMove.AsReadOnly(); }
    }

    private void Start()
    {
        var enumValues = Enum.GetNames(typeof(ObstaclePoolItem));

        foreach (var item in _pooledGameObjects)
        {
            if (!enumValues.Contains(item.name))
            {
                Debug.LogError($"There is no {item.name} type", gameObject);
            }
        }

        foreach (var item in enumValues)
        {
            if (_pooledGameObjects.Any(gameObject => gameObject.name == item))
                continue;
            Debug.LogError($"There is no {item} object", gameObject);
        }


        foreach (var item in _pooledGameObjects)
        {
            var poolKey = item.GetComponent<ObstaclePoolableKey>().PoolKey;
            if (_factory.ContainsKey(poolKey))
            {
                Debug.LogError($"Factory already have {item.name} key", gameObject);
                continue;
            }
            var parent = new GameObject($"Pool_{item.name}");
            parent.transform.SetParent(transform);
            _factory.Add(poolKey, () =>
            {
                var go = Instantiate(item);
                go.transform.SetParent(parent.transform);
                go.SetActive(false);
                return go;
            });
        }


        foreach (var item in _pooledGameObjects)
        {
            var poolKey = item.GetComponent<ObstaclePoolableKey>().PoolKey;

            if (_dictionaryPool.ContainsKey(poolKey))
            {
                Debug.LogError($"Dictionary Pool already have {item.name} key", gameObject);
                continue;
            }

            var pooledStack = new Stack<GameObject>(_poolSize);
            for (int i = 0; i < _poolSize; i++)
            {
                if (_factory.TryGetValue(poolKey, out var build))
                {
                    pooledStack.Push(build());
                }
                else
                {
                    Debug.LogError($"????", gameObject);
                }
            }
            _dictionaryPool.Add(poolKey, pooledStack);
        }
    }

    public GameObject TakeObstacle(ObstaclePoolItem ObstacleType)
    {
        if (_dictionaryPool.TryGetValue(ObstacleType, out Stack<GameObject> obstacleStack))
        {
            if (obstacleStack.Count > 0)
            {
                GameObject go = obstacleStack.Pop();
                _obstacleOnScene.Add(go);
                _obstacleOnSceneDirMove.Add(go.GetComponent<DirectionalMovement>());
                go.SetActive(true);
                return go;
            }

            GameObject factoryGo = _factory[ObstacleType]();
            _obstacleOnScene.Add(factoryGo);
            _obstacleOnSceneDirMove.Add(factoryGo.GetComponent<DirectionalMovement>());
            factoryGo.SetActive(true);
            return factoryGo;
        }
        throw new ArgumentException($"{ObstacleType} does not exist");
    }

    public void ReleaseObstacle(GameObject ObstacleToRelease)
    {
        var poolKeyComp = ObstacleToRelease.GetComponentInParent<ObstaclePoolableKey>();
        if (poolKeyComp != null)
        {
            var poolableArray = ObstacleToRelease.GetComponentsInParent<IPoolableMonobehaviour>();
            if (poolableArray != null && poolableArray.Length > 0)
            {
                foreach (var item in poolableArray)
                {
                    item.Release();
                }
            }
            poolKeyComp.gameObject.transform.position = transform.position;
            poolKeyComp.gameObject.SetActive(false);
            _obstacleOnSceneDirMove.RemoveAt(_obstacleOnScene.IndexOf(poolKeyComp.gameObject));
            _obstacleOnScene.Remove(poolKeyComp.gameObject);
            _dictionaryPool[poolKeyComp.PoolKey].Push(poolKeyComp.gameObject);
        }
        else
            Destroy(ObstacleToRelease);
    }

    public DirectionalMovement GetDirMoveSpeedComponent(GameObject Go)
    {
       return _obstacleOnSceneDirMove[_obstacleOnScene.IndexOf(Go)];
    }
}

 public enum ObstaclePoolItem
{
    Obstacle_Branch,
    Obstacle_Log,
    Obstacle_Rock,
}
