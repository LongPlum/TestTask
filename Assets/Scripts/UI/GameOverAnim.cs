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
            .Append(_gameOverText.transform.DOScaleY(_gameOverText.transform.localScale.y * 2 , 1))
            .Append(_gameOverText.transform.DOScaleY(_gameOverText.transform.localScale.y, 1))
            .Insert(0, _gameOverText.transform.DOShakeScale(2));
        _gameOverTextTween.SetLoops(-1);
        _gameOverTextTween.Play();
    }

}
