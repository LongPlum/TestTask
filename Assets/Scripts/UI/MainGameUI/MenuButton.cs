using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private GameObject MainMenu;
    [SerializeField] PauseManager _pauseManager;

    public void EnableMenu()
    {
        MainMenu.SetActive(true);
        _pauseManager.Pause();
    }
    
}
