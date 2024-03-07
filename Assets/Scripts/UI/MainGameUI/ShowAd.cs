using System.Collections;
using UnityEngine;

public class ShowAd : MonoBehaviour
{
    [SerializeField] private PauseManager _pauseManager;
    [SerializeField] private AdManager _adManager;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private float _secToShowAd = 7;

    private Coroutine _adCoroutine;
    private bool _isAdWasUsed;

    private void Awake()
    {
        _levelManager.GameStarted += WaitToShowAd;
        _adManager.RewardPlayer += PlayerRewarded;
        _pauseManager.GamePaused += StopAdCoroutine;
        _pauseManager.GameResumed += WaitToShowAd;
    }

    public void PlayerRewarded()
    {
        _levelManager.Score *= 2;
        _pauseManager.Resume();
        gameObject.SetActive(false); 
        _isAdWasUsed = true;
    }

    private void WaitToShowAd()
    {
        if (!_isAdWasUsed && _levelManager.IsLevelBegin)
        {
            _adCoroutine = StartCoroutine(ShowAdCoroutine());
        }
    }

    private void StopAdCoroutine()
    {
        if(_adCoroutine != null)
        StopCoroutine(_adCoroutine);
    }

    private IEnumerator ShowAdCoroutine()
    {
        _secToShowAd = _secToShowAd - Mathf.Round( _levelManager.GameTime);
        yield return new WaitForSeconds(_secToShowAd);
        _pauseManager.Pause();
        _adManager.PlayRewardedAd();
    }
}
