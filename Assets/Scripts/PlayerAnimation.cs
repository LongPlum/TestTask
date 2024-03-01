using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;

    private static readonly int _jump = Animator.StringToHash("Jump");
    private static readonly int _run = Animator.StringToHash("Run");
    private static readonly int _slide = Animator.StringToHash("Slide");



    public void PlayerJump()
    {
        _playerAnimator.CrossFadeInFixedTime(_jump, 0.2f);
    }

    public void StartRunning()
    {
        _playerAnimator.CrossFadeInFixedTime(_run, 0.2f);
    }

    public void PlayerSlide()
    {
        _playerAnimator.CrossFadeInFixedTime(_slide, 0.2f);
    }

    public void PlayerDeath()
    {
    }

}
