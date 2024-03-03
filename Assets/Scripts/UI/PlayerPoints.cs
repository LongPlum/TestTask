using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerPoints : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;
    private TextMeshPro _score;
    private bool _isGameStarted;

    void Start()
    {
        _levelManager.GameStarted += GameStart;
        _score = gameObject.GetComponent<TextMeshPro>();
    }

    void Update()
    {
        if (_isGameStarted)
        {
            _score.text = _levelManager.Score.ToString();
        }
    }

    private void GameStart()
    {
        _isGameStarted = true;
    }    

}
