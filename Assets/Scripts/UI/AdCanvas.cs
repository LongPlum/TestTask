using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdCanvas : MonoBehaviour
{
    [SerializeField] private PauseManager _pauseManager;
    [SerializeField] private AdManager _adManager;

    private void OnEnable()
    {
        _pauseManager.Pause(); 
        _adManager.PlayRewardedAd();
    }

    private void OnDisable()
    {
        _pauseManager.Resume();
    }
}
