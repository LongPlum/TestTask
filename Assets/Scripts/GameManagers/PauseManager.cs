using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public event Action GamePaused;
    public event Action GameResumed;

    public void Pause()
    {
        Time.timeScale = 0;
        GamePaused.Invoke();
    }

    public void Resume()
    {
        Time.timeScale = 1;
        GameResumed.Invoke();
    }
}
