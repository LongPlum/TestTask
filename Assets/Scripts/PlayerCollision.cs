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

    public event Action<float> OnPlayerCollision;

    private void Start()
    {
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.TryGetComponent<Obstacle>(out Obstacle obs))
        {
            _obstaclePool.ReleaseObstacle(obs.gameObject);


            foreach (var Obstacle in _obstaclePool.ObstacleOnScene)
            {
                var movementComponent = Obstacle.GetComponent<DirectionalMovement>();
                var obstacleSpeed = movementComponent.ObstacleMoveSpeed;
                movementComponent.ObstacleMoveSpeed = 0;

                Obstacle.transform.DOMoveZ(Obstacle.transform.position.z + _knockBackDistance, _knockBackDuration)
                    .SetEase(Ease.OutExpo)
                    .Play()
                    .SetAutoKill(true)
                    .OnUpdate(() => movementComponent.ObstacleMoveSpeed = obstacleSpeed)
                    .OnKill(() => movementComponent.ObstacleMoveSpeed = obstacleSpeed);
            }

            OnPlayerCollision.Invoke(_knockBackDuration);
    
        }
    
    }
    
}
