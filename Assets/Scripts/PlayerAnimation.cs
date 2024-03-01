using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;

    public void PlayerJump()
    {
        _playerAnimator.SetTrigger("Jump");
    }
}
