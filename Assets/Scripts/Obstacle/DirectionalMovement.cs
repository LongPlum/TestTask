using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalMovement : MonoBehaviour
{
    public float ObstacleMoveSpeed { get; set; }

    void Update()
    {
        transform.Translate(ObstacleMoveSpeed * Time.deltaTime * Vector3.back);
    }
}
