using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachinePlayerMovement : MonoBehaviour
{
    [SerializeField] private float _gravity = 7f;
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _bounds = 2f;
    [SerializeField] private float _minDistanceForSwipe = 0.2f;
    [SerializeField] private ShowAd _ad;
    [SerializeField] private LevelManager _levelManager;


    private PlayerAnimation _playerAnimation;
    private PlayerCollision _playerCollision;

    private bool _isOnJump;
    private bool _isOnSlide;
    private float _moveHorizontal;
    private float _moveVertical;
    private float _gravityForce;
    private float _playerGroundPosY;
    private float _defaultGravity;
    private bool _isUnderGround;
    private bool _isLeftBorder;
    private bool _isRightBorder;
    private Coroutine _slideCoroutine;

    private Vector2 _beginTouchPos;
    private Vector2 _moveTouchPos;
    private Vector2 _swipeDirection;
    private Vector3 _nextPlayerPos;

    private MovementState _state;


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
            case TouchPhase.Ended:
                _moveVertical = _moveHorizontal = 0;
                break;
            default:
                break;
        }
    }

    private void PlayerInput()
    {

#if UNITY_EDITOR
        PlayerKeyboardInput();
#endif
        if (Input.touchCount == 1)
        {
            PlayerPhoneInput(Input.GetTouch(0));
        }
    }

    private void CalculateGravity()
    {
       _gravityForce -= _gravity * Time.deltaTime;
        transform.Translate(0, _gravityForce * Time.deltaTime, 0);
    }


    private void CheckHorizontalBorders()
    {
        _nextPlayerPos.x = _moveHorizontal * _moveSpeed * Time.deltaTime + transform.position.x;
        _isLeftBorder = _nextPlayerPos.x < -_bounds;
        _isRightBorder = _nextPlayerPos.x > _bounds;
        
        if (_isRightBorder || _isLeftBorder)
        {
            var posCur = transform.position;
            posCur.x = _isRightBorder ? _bounds : -_bounds;
            transform.position = posCur;
        }
        
    }
    private void CheckVerticalBorders()
    {
        _nextPlayerPos.y = _gravityForce * Time.deltaTime + transform.position.y;

        _isUnderGround = _nextPlayerPos.y < _playerGroundPosY;

        if (_isUnderGround)
            {
                var posCur = transform.position;
                posCur.y = _playerGroundPosY;
                transform.position = posCur;
            }
    }

    private void HorizontalMovement()
    {
        CheckHorizontalBorders();

        if (_moveHorizontal != 0 && !_isRightBorder && !_isLeftBorder)
        {
            transform.Translate(_moveHorizontal * _moveSpeed * Time.deltaTime, 0, 0);
        }
    }

    private IEnumerator WaitForSlide()
    {
        yield return new WaitForSeconds(0.7f);
        _isOnSlide = false;
        _state = MovementState.Run;
    }

    private void GameStart()
    {
        _playerAnimation.StartRunning();
        _state = MovementState.Run;
    }

    private void GameOver()
    {
        _playerAnimation.PlayerDeath();
        if (_slideCoroutine != null)
        {
            StopCoroutine(_slideCoroutine);
        }
        _state = MovementState.Death;
    }
    

    private void Start()
    {
        _playerAnimation = GetComponent<PlayerAnimation>();
        _playerCollision = GetComponent<PlayerCollision>();

        _state = MovementState.Idle;
        _defaultGravity = _gravity;

        _levelManager.GameStarted += GameStart;
        _ad.Resurrection += GameStart;
        _playerCollision.GameOver += GameOver;
        _playerGroundPosY = transform.position.y;
    }

    private void Update()
    {

        switch (_state)
        {
            case  MovementState.Idle:
                break;

            case MovementState.Run:

                PlayerInput();
                HorizontalMovement();

                if (_moveVertical > 0 )
                {
                    _state = MovementState.Jump;
                }
                else if (_moveVertical < 0)
                {
                    _playerAnimation.PlayerSlide();
                    _state = MovementState.Slide;
                }
                break;

            case MovementState.Jump:

                if (!_isOnJump)
                {
                    _playerAnimation.PlayerJump();
                    _isOnJump = true;
                    _gravityForce = _jumpForce;
                }

                CalculateGravity();
                PlayerInput();
                HorizontalMovement();

                if(_moveVertical < 0)
                {
                    _gravity += 10;
                }

                CheckVerticalBorders();
                if (_isUnderGround)
                {
                    _isOnJump = false;
                    _gravity = _defaultGravity;
                    _state = MovementState.Run;
                }
                break;

            case MovementState.Slide:

                PlayerInput();
                if (!_isOnSlide)
                { 
                    _isOnSlide = true;
                    _slideCoroutine = StartCoroutine(WaitForSlide());
                }
                break;

            case MovementState.Death:
                break;

            default:
                break;
        }
    }
}

enum MovementState
{
    Idle,
    Run,
    Jump,
    Slide,
    Death
}