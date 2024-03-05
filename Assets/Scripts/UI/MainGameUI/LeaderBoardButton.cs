using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardButton : MonoBehaviour
{
    [SerializeField] private GameObject _leaderBoard;
    public void LeaderBoardClick()
    {
        _leaderBoard.SetActive(true);
    }
    
}
