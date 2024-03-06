using System.Collections;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using Firebase;
using UnityEngine.SceneManagement;
using System;
using Firebase.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class FireBaseManager : MonoBehaviour
{
    private LevelManager _levelManager;
    private DatabaseReference _databaseReference;
    private FirebaseAuth _auth;
    private FirebaseUser _user;

    public event Action<string> ErrorEvent;

    public static FireBaseManager FireBaseManagerInstance { get; private set; }

    private void Awake()
    {
        _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        _auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        FireBaseManagerInstance = this;
        DontDestroyOnLoad(FireBaseManagerInstance);
    }


    public void SafeScore()
    {
        _levelManager = FindObjectOfType<LevelManager>();

        _databaseReference.Child("Users").Child(_user.DisplayName).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Failed");
            }
            else if (task.IsCompleted)
            {
                if (task.Result.Value == null || Convert.ToSingle(task.Result.Value) < _levelManager.Score)
                {
                    _databaseReference.Child("Users").Child(_user.DisplayName).SetValueAsync(Convert.ToDouble(_levelManager.Score));
                }
            }
        });
    }

    public async Task<Dictionary<string, double>> TakeUsersScoreAsync()
    {
        Dictionary<string, double> dataDictionary = new ();
        var result = await _databaseReference.Child("Users").GetValueAsync();

        IEnumerable<DataSnapshot> snapshotChildren = result.Children;
        foreach (DataSnapshot childSnapshot in snapshotChildren)
        {
            dataDictionary.Add(childSnapshot.Key, Convert.ToDouble(childSnapshot.Value));
        }
        var sortedByValue = dataDictionary.OrderByDescending(x => x.Value);
        dataDictionary = new Dictionary<string, double>(sortedByValue);

        return dataDictionary;
    }



    public IEnumerator Login(string Email, string Password)
    {
        var LoginTask = _auth.SignInWithEmailAndPasswordAsync(Email, Password);

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
            ErrorEvent.Invoke(message);
        }
        else
        {
            _user = LoginTask.Result.User;
            SceneManager.LoadScene("Game");
        }
    }

    public IEnumerator Register(string Email, string Password, string UserName)
    {
        {
            var RegisterTask = _auth.CreateUserWithEmailAndPasswordAsync(Email, Password);

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
                ErrorEvent.Invoke(message);
            }
            else
            {
                _user = RegisterTask.Result.User;

                if (_user != null)
                {
                    UserProfile profile = new UserProfile { DisplayName = UserName };

                    var ProfileTask = _user.UpdateUserProfileAsync(profile);

                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        ErrorEvent.Invoke("Username Set Failed!");

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