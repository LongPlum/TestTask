using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerPointsUI : MonoBehaviour
{
    [SerializeField] private LevelManager _levelManager;
    private TMP_Text _score;
    private bool _isGameStarted;

    void Start()
    {
        _levelManager.GameStarted += GameStart;
        _score = gameObject.GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (_isGameStarted)
        {
            _score.text = _levelManager.Score.ToString("0.");
        }
    }

    private void GameStart()
    {
        _isGameStarted = true;
    }    

}
