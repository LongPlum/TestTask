using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour
{

    public event Action RewardPlayer;

    private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
    private RewardedAd _rewardedAd;

    private void Awake()
    {
        MobileAds.Initialize(InitializationStatus => { });
    }

    private void LoadRewardedAd()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        var AdRequest = new AdRequest();

        RewardedAd.Load(_adUnitId, AdRequest, (RewardedAd Ad, LoadAdError Error) =>
            {
                if (Error != null || Ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + Error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + Ad.GetResponseInfo());
                _rewardedAd = Ad; 
            });
        
    }

   public void PlayRewardedAd()
    {
        LoadRewardedAd();
        if (_rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) => RewardPlayer.Invoke());
        }
    }

 
}
