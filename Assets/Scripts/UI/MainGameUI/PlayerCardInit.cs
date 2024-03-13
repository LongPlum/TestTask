using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerCardInit : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _score;
    
    public void Init(string Name, float Score)
    {
        _name.text = Name;
        _score.text = Score.ToString();
    }
}
