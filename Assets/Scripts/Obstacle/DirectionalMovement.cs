using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalMovement : MonoBehaviour
{
    public float MoveSpeed { get; set; }

    void Update()
    {
        transform.Translate(MoveSpeed * Time.deltaTime * Vector3.back);
    }
}
