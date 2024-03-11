using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
   public void RestartGame()
    {
        FireBaseManager.FireBaseManagerInstance.SafeScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
