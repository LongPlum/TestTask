using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] PauseManager _pauseManager;

    private void OnEnable()
    {
        _pauseManager.Pause();
    }

    private void OnDisable()
    {
        _pauseManager.Resume();
    }

}
