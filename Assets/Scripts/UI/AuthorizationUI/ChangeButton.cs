using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeButton : MonoBehaviour
{
    [SerializeField] private GameObject _registrationCanvas;
    [SerializeField] private GameObject _loginCanvas;

   public void ChangeCanvas()
    {
        if (_registrationCanvas.activeSelf)
        {
            _registrationCanvas.SetActive(false);
            _loginCanvas.SetActive(true);
        }
        else
        {
            _registrationCanvas.SetActive(true);
            _loginCanvas.SetActive(false);
        }

    }
}
