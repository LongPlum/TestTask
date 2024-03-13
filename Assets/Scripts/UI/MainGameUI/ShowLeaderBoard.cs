using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ShowLeaderBoard : MonoBehaviour
{
    private ScrollRect _scrollRect;
    private List<GameObject> _playerCardList = new();
    [SerializeField] private GameObject _playerCard;

    private void Start()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }
    private async void OnEnable()
    {
        Dictionary<string, double> dataDictionary =
            await FireBaseManager.FireBaseManagerInstance.TakeUsersScoreAsync();

        foreach (var pair in dataDictionary)
        {
            var PlayerCard = Instantiate(_playerCard, _scrollRect.content.transform);
            _playerCardList.Add(PlayerCard);
            PlayerCard.GetComponent<PlayerCardInit>().Init(pair.Key, Convert.ToSingle(pair.Value));
            _scrollRect.content.SetParent(PlayerCard.transform);
        }
    }

    private void OnDisable()
    {
        foreach (var PlayerCard in _playerCardList)
        {
            Destroy(PlayerCard);
        }
    }
}
