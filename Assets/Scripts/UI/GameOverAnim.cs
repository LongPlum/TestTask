using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;


public class GameOverAnim : MonoBehaviour
{
    private TextMeshPro _gameOverText;
    private Sequence _gameOverTextTween;

    void OnEnable()
    {
        _gameOverText = gameObject.GetComponent<TextMeshPro>();
        _gameOverTextTween = DOTween.Sequence()
            .Append(_gameOverText.transform.DOScaleY(_gameOverText.transform.position.y * 2, 1))
            .Append(_gameOverText.transform.DOScaleY(_gameOverText.transform.position.y, 1))
            .Insert(0, _gameOverText.transform.DOShakeScale(2));
        _gameOverTextTween.Play();
    }

}
