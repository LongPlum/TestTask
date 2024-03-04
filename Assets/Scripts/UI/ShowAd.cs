using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAd : MonoBehaviour
{
    [SerializeField] private PauseManager _pauseManager;
    [SerializeField] private AdManager _adManager;

    private void Awake()
    {
        _adManager.RewardPlayer += PlayerRewarded;    
    }

    private void OnEnable()
    {
        _pauseManager.Pause(); 
        _adManager.PlayRewardedAd();
    }

    public void PlayerRewarded()
    {
        _pauseManager.Resume();
        gameObject.SetActive(false);
    }
}
