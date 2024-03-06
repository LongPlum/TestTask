using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Registration : MonoBehaviour
{
    [SerializeField] FireBaseManager _fireBaseManager;

    [SerializeField] private TMP_InputField _emailRegisterField;
    [SerializeField] private TMP_InputField _loginRegisterField;
    [SerializeField] private TMP_InputField _passwordRegisterField;
    [SerializeField] private TMP_InputField _passwordRegisterVerifyField;
    [SerializeField] private TMP_Text _warningRegisterText;


    private void Start()
    {
        _fireBaseManager.ErrorEvent += (text) => _warningRegisterText.text = text;
    }
    public void RegisterButton()
    {
        if (_loginRegisterField.text == "")
        {
            _warningRegisterText.text = "Missing Username";
        }
        else if (_passwordRegisterField.text != _passwordRegisterVerifyField.text)
        {
            _warningRegisterText.text = "Password Does Not Match!";
        }
        StartCoroutine(_fireBaseManager.Register(_emailRegisterField.text, _passwordRegisterField.text, _loginRegisterField.text));
    }
}
