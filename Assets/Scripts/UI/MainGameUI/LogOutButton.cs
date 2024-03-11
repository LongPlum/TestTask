using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LogOutButton : MonoBehaviour
{
    public void LogOutClick()
    {
        FireBaseManager.FireBaseManagerInstance.SafeScore();
        FireBaseManager.FireBaseManagerInstance.LogOut();
        SceneManager.LoadScene("Authorization");
    }
}
