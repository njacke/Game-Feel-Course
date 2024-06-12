using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 MoveInput => _frameInput.Move;

    public static Action OnJump;
    public static PlayerController Instance;

    [SerializeField] private Transform _feetTransform; // drag object in
    [SerializeField] private Vector2 _groundCheck; // modify in inspector
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _jumpStrength = 7f;
    [SerializeField] private float _extraGravity = 700f;
    [SerializeField] private float _gravityDelay = .2f;
    [SerializeField] private float _coyoteTime = .5f;
    
    private float _timeInAir, _coyoteTimer;
    private bool _doubleJumpAvailable;

    private PlayerInput _playerInput;
    private FrameInput _frameInput;

    private Rigidbody2D _rigidBody;
    private Movement _movement;

    public void Awake() {
        if (Instance == null) { Instance = this; }

        _rigidBody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _movement = GetComponent<Movement>();
    }

    private void OnEnable() {
        OnJump += ApplyJumpForce;
    }

    private void OnDisable() {
        OnJump -= ApplyJumpForce;
    }

    private void Update() {
        GatherInput();
        Movement();
        CoyoteTimer();
        HandleJump();
        HandleSpriteFlip();
        GravityDelay();
    }


    private void FixedUpdate() {
        ExtraGravity();
    }

    public bool CheckGrounded() {
        // returns true if it collides with _groundlayer
        Collider2D _isGrounded = Physics2D.OverlapBox(_feetTransform.position, _groundCheck, 0f, _groundLayer);
        return _isGrounded;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_feetTransform.position, _groundCheck);
    }

    private void GravityDelay() {
        if (!CheckGrounded()) {
            _timeInAir += Time.deltaTime;
        } else {
            _timeInAir = 0f;
        }
    }

    private void ExtraGravity() {
        if (_timeInAir > _gravityDelay) {
            _rigidBody.AddForce(new Vector2(0f, -_extraGravity * Time.deltaTime));
        }
    }

    private void GatherInput() {
        _frameInput = _playerInput.FrameInput;
    }

    private void Movement() {
        _movement.SetCurrentDirection(_frameInput.Move.x);
    }

    private void HandleJump() {
        if (!_frameInput.Jump) { return; }

        if (CheckGrounded()) {
            OnJump?.Invoke();
        } else if (_coyoteTimer > 0f) {
            OnJump?.Invoke();
        } else if (_doubleJumpAvailable) {
            _doubleJumpAvailable = false;
            OnJump?.Invoke();
        }
    }

    private void CoyoteTimer() {
        if (CheckGrounded()) {
            _coyoteTimer = _coyoteTime;
            _doubleJumpAvailable = true;
        } else {
            _coyoteTimer -= Time.deltaTime;
        }
    }

    private void ApplyJumpForce() {
         // reset so custom gravity doesn't affect it
        _rigidBody.velocity = Vector2.zero;
        _timeInAir = 0f;
        _coyoteTimer = 0f; // fail safe if spamming jump
        _rigidBody.AddForce(Vector2.up * _jumpStrength, ForceMode2D.Impulse);
    }

    private void HandleSpriteFlip() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    } 
}
