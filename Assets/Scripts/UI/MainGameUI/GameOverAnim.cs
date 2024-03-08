using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;


public class GameOverAnim : MonoBehaviour
{
    private TMP_Text _gameOverText;
    private Sequence _gameOverTextTween;

    void OnEnable()
    {
        _gameOverText = gameObject.GetComponent<TMP_Text>();
        _gameOverTextTween = DOTween.Sequence()
            .Append(_gameOverText.transform.DOScaleY(_gameOverText.transform.localScale.y + 1.8f, 0.3f))
            .Insert(0, _gameOverText.transform.DOScaleX(_gameOverText.transform.localScale.x + 1.8f, 0.3f))
            .Append(_gameOverText.transform.DOScaleY(_gameOverText.transform.localScale.y + 1.5f, 1))
            .Insert(0.3f, _gameOverText.transform.DOScaleX(_gameOverText.transform.localScale.x + 1.5f, 1))
            .Insert(0.3f, _gameOverText.transform.DOShakeScale(1, 2));
        _gameOverTextTween.Play();
    }
    private void OnDisable()
    {
        _gameOverTextTween.Kill();
    }

}