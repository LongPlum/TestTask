using System;
using System.Collections;
using UnityEngine;

public class ShowAd : MonoBehaviour
{
    [SerializeField] private PauseManager _pauseManager;
    [SerializeField] private AdManager _adManager;
    [SerializeField] private GameObject _gameOverMenu;

    private bool _adWasPlayed;
    public event Action Resurrection;

    private void Awake()
    {
        _adManager.RewardPlayer += PlayerRewarded;
    }

    public void PlayerRewarded()
    {
        _pauseManager.Resume();
        _gameOverMenu.SetActive(false);
        Resurrection.Invoke();
    }

    public void PlayAdOnClick()
    {
        if (!_adWasPlayed)
        {
            _adManager.PlayRewardedAd();
            _adWasPlayed = true;
        }
    }
}
