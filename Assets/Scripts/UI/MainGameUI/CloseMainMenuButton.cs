using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseMainMenuButton : MonoBehaviour
{
    [SerializeField] private PauseManager _pausedManager;
    public void CloseMenuClick()
    {
        gameObject.transform.parent.gameObject.transform.parent.gameObject.SetActive(false);
        _pausedManager.Resume();
    }
}
