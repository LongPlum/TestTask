using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] PauseManager _pauseManager;
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
#endif
        if (Input.touchCount > 1)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _pauseManager.Pause();
    }

    private void OnDisable()
    {
        _pauseManager.Resume();
    }

}
