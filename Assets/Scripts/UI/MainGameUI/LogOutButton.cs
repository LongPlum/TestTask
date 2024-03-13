using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LogOutButton : MonoBehaviour
{
    [SerializeField] PauseManager _pauseManager;
    public void LogOutClick()
    {
        FireBaseManager.FireBaseManagerInstance.SafeScore();
        FireBaseManager.FireBaseManagerInstance.LogOut();
        _pauseManager.Resume();

        SceneManager.LoadScene("Authorization");
    }
}
