using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public void Exit()
    {
        FireBaseManager.FireBaseManagerInstance.SafeScore();
        Application.Quit();
    }
}
