using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePoolableKey : MonoBehaviour
{
     public ObstaclePoolItem poolKey { get => key; }
    [SerializeField] private ObstaclePoolItem key;
}
