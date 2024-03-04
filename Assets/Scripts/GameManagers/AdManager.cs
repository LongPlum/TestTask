using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour
{

    public Action RewardPlayer;

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

        var adRequest = new AdRequest();

        RewardedAd.Load(_adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedAd = ad; 
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
