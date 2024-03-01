using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float _gravity = 7f;
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _bounds = 2f;
    [SerializeField] private float _rollDuration = 2f;
    [SerializeField] private float _minDistanceForSwipe =0.5f;
    [SerializeField] private PlayerAnimation _playerAnimation;



    private SphereCollider _playerCollider;
    private Sequence _slideTween;
    private bool _isJump;
    private float _moveHorizontal;
    private float _moveVertical;
    private float _gravityAcc;
    private float _playerColliderR;
    private bool _isGrounded;
    private bool _isUnderGround;
    private bool _isLeftBorder;
    private bool _isRightBorder;
    private Vector2 _beginTouchPos;
    private Vector2 _moveTouchPos;
    private Vector2 _swipeDirection;




    private void PlayerKeyboardInput()
    {
        _moveHorizontal = Input.GetAxis("Horizontal");
        _moveVertical = Input.GetAxis("Vertical");
    }

    private void PlayerPhoneInput(Touch touchInput)
    {
        switch (touchInput.phase)
        {
            case TouchPhase.Began:
                _beginTouchPos = touchInput.position;
                break;
            case TouchPhase.Moved:
                _moveTouchPos = touchInput.position;
                if (Vector2.Distance(_beginTouchPos, _moveTouchPos) > _minDistanceForSwipe)
                {
                   _swipeDirection = _beginTouchPos - _moveTouchPos;

                    if (Mathf.Abs(_swipeDirection.x) > Mathf.Abs(_swipeDirection.y))
                    {
                        _moveHorizontal = _swipeDirection.normalized.x * -1;
                    }
                    else
                    {
                        _moveVertical = _swipeDirection.normalized.y * -1;
                    }
                }
                break;
            default:
                break;
        }
    }


    private float CalculateGravity()
    {
        return _gravity * Time.deltaTime;
    }

    private Vector3 CalculateDir()
    {
       return new Vector3(_moveHorizontal * _moveSpeed * Time.deltaTime, _gravityAcc * Time.deltaTime, 0);
    }

    private void CheckNextPos(Vector3 posNext)
    {
        _isGrounded = _isUnderGround = posNext.y <= _playerColliderR;
        _isLeftBorder = posNext.x <= -_bounds;
        _isRightBorder = posNext.x >= _bounds;
    }


    private void Start()
    {
       // _playerCollider = gameObject.GetComponent<SphereCollider>();
        //_playerColliderR = _playerCollider.radius;

        /*
        _slideTween = DOTween.Sequence()
      .Append(transform.DOScaleY(transform.localScale.y / 2, _rollDuration / 2))
      .Append(transform.DOScaleY(transform.localScale.y, _rollDuration / 2))
      .Insert(0, transform.DOMoveY(_playerColliderR / 2, _rollDuration / 2))
      .Insert(_rollDuration / 2, transform.DOMoveY(_playerColliderR, _rollDuration / 2));
        */
    }

    private void Update()
    {
#if UNITY_EDITOR
        PlayerKeyboardInput();
#else
        if (Input.touchCount == 1)
        {
         PlayerPhoneInput(Input.GetTouch(0));
        }
#endif

        _gravityAcc -= CalculateGravity();
        var dir = CalculateDir();
        var posCur = transform.position;
        var posNext = dir + posCur;

        CheckNextPos(posNext);

        if (_isRightBorder || _isLeftBorder)
        {
            if (_isLeftBorder)
            {
                posCur = transform.position;
                posCur.x = -_bounds;
                transform.position = posCur;
            }
            else if (_isRightBorder)
            {
                posCur = transform.position;
                posCur.x = _bounds;
                transform.position = posCur;
            }

            dir.x = 0;
        }


        if (_isGrounded && !_isJump)
        {
            dir.y = 0;
            _gravityAcc = Mathf.Max(0, _gravityAcc);
        }

        if (dir != Vector3.zero)
        {
            transform.Translate(dir);
            _moveHorizontal = _moveVertical = 0;
            _isJump = false;
        }

        if (_isGrounded)//&& !_slideTween.IsPlaying()
        {
            if (_moveVertical < 0)
                Debug.Log("aboba")
;                //_slideTween.Restart();
            else if (_moveVertical > 0 && !_isJump)
            {
                _gravityAcc = _jumpForce;
                _isJump = true;
                _playerAnimation.PlayerJump();
            }
        }

        if (_isUnderGround)
        {
            posCur = transform.position;
            posCur.y = _playerColliderR;
            transform.position = posCur;
        }


    }
}

