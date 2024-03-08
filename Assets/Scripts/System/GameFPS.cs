using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFPS : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 200;
    }
}
