using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float _gravity = 7f;
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _bounds = 2f;
    [SerializeField] private float _minDistanceForSwipe = 0.5f;
    [SerializeField] private PlayerAnimation _playerAnimation;
    [SerializeField] private PlayerCollision _playerCollision;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private BoxCollider _playerCollider;


    private bool _isJump;
    private float _moveHorizontal;
    private float _moveVertical;
    private float _gravityAcc;
    private float _playerColliderY;
    private bool _isGrounded;
    private bool _isUnderGround;
    private bool _isLeftBorder;
    private bool _isRightBorder;
    private Vector2 _beginTouchPos;
    private Vector2 _moveTouchPos;
    private Vector2 _swipeDirection;
    private bool _isMovementAllowed;




    private void PlayerKeyboardInput()
    {
        _moveHorizontal = Input.GetAxis("Horizontal");
        _moveVertical = Input.GetAxis("Vertical");
    }

    private void PlayerPhoneInput(Touch TouchInput)
    {
        switch (TouchInput.phase)
        {
            case TouchPhase.Began:
                _beginTouchPos = TouchInput.position;
                break;
            case TouchPhase.Moved:
                _moveTouchPos = TouchInput.position;
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


    private IEnumerator SlideEnableMovement()
    {
        yield return new WaitForSeconds(1);
        EnableMovement();
    }

    private float CalculateGravity()
    {
        return _gravity * Time.deltaTime;
    }

    private Vector3 CalculateDir()
    {
        return new Vector3(_moveHorizontal * _moveSpeed * Time.deltaTime, _gravityAcc * Time.deltaTime, 0);
    }

    private void CheckNextPos(Vector3 PosNext)
    {
        _isGrounded = _isUnderGround = PosNext.y <= _playerColliderY;
        _isLeftBorder = PosNext.x <= -_bounds;
        _isRightBorder = PosNext.x >= _bounds;
    }


    private void DisableMovement()
    {
        _isMovementAllowed = false;
    }
    private void EnableMovement()
    {
        _isMovementAllowed = true;
    }


    private void Start()
    {
        _levelManager.GameStarted += EnableMovement;
        _playerCollision.GameOver += DisableMovement;
        _playerCollision.GameOver += StopAllCoroutines;
        _playerColliderY = _playerCollider.transform.localPosition.y; 
    }

    private void Update()
    {
        if (_isMovementAllowed)
        {
#if UNITY_EDITOR
            PlayerKeyboardInput();
#endif
            if (Input.touchCount == 1)
            {
                PlayerPhoneInput(Input.GetTouch(0));
            }


            _gravityAcc -= CalculateGravity();
            var dir = CalculateDir();
            var posCur = transform.position;
            var posNext = dir + posCur;

            CheckNextPos(posNext);


            if (_isRightBorder || _isLeftBorder)
            {
                posCur = transform.position;
                posCur.x = _isRightBorder ? _bounds : -_bounds;
                transform.position = posCur;
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


            if (_isGrounded)
            {
                if (_moveVertical < 0) 
                {
                    _playerAnimation.PlayerSlide();
                    DisableMovement();
                    StartCoroutine(SlideEnableMovement());
                }

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
                posCur.y = _playerColliderY;
                transform.position = posCur;
            }


        }
    }
}

