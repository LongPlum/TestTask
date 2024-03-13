using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Login : MonoBehaviour
{
    [SerializeField] private FireBaseManager _fireBaseManager;

    [SerializeField] private TMP_InputField _emailLoginField;
    [SerializeField] private TMP_InputField _passwordLoginField;
    [SerializeField] private TMP_Text _warningLoginText;

    private void Start()
    {
        _fireBaseManager.ErrorEvent += (text) => _warningLoginText.text = text;
    }

    public void LoginButton()
    {
        StartCoroutine(_fireBaseManager.Login(_emailLoginField.text, _passwordLoginField.text));
    }

}
