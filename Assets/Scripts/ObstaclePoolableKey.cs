using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePoolableKey : MonoBehaviour
{
     public ObstaclePoolItem PoolKey { get => _key; }
    [SerializeField] private ObstaclePoolItem _key;
}
