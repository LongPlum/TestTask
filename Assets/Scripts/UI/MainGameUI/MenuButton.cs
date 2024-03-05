using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private GameObject MainMenu;

    public void EnableMenu()
    {
        MainMenu.SetActive(MainMenu.activeSelf ? false : true);
    }
    
}
