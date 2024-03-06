using System.Collections;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using Firebase;
using UnityEngine.SceneManagement;
using System;
using Firebase.Extensions;

public class FireBaseManager : MonoBehaviour
{
    private LevelManager _levelManager;
    private DatabaseReference _databaseReference;
    private FirebaseAuth _auth;
    private FirebaseUser _user;

    public Action<string> ErrorHappens;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        _auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    public void SafeScore()
    {
        if (_levelManager == null)
        {
            _levelManager = GetComponent<LevelManager>();
        }
        _databaseReference.Child("Users").Child(_user.DisplayName).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Failed");
            }
            else if (task.IsCompleted)
            {
                float DbScore = (float)task.Result.Value;
                if (DbScore < _levelManager.Score)
                {
                    _databaseReference.Child("Users").Child(_user.DisplayName).Child(_levelManager.Score.ToString());
                }
            }
        });
        
    }

    public void TakeUsersScore()
    {
        _databaseReference.Child("Users").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Failed");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
            }
        });
    }

    public IEnumerator Login(string _email, string _password)
    {
        var LoginTask = _auth.SignInWithEmailAndPasswordAsync(_email, _password);

        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            ErrorHappens.Invoke(message);
        }
        else
        {
            _user = LoginTask.Result.User;
            SceneManager.LoadScene("Game");
        }
    }

    public IEnumerator Register(string _email, string _password, string _username)
    {
        {
            var RegisterTask = _auth.CreateUserWithEmailAndPasswordAsync(_email, _password);

            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                ErrorHappens.Invoke(message);
            }
            else
            {
                _user = RegisterTask.Result.User;

                if (_user != null)
                {
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    var ProfileTask = _user.UpdateUserProfileAsync(profile);

                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        ErrorHappens.Invoke("Username Set Failed!");

                    }
                    else
                    {
                        SceneManager.LoadScene("Game");
                    }
                }
            }
        }
    }

}
