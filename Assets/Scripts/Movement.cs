using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public float movementScalar = 16;
    
    public float dashSpeed = 50f;
    [Header("Delay in seconds between each dash")]
    [Range(0.0f, 5.0f)]
    public float dashDelay = 0f;
    public static event Action OnPlayerMovement;
    
    private Rigidbody2D _rb2d;
    private Animator _animator;
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Magnitude = Animator.StringToHash("Magnitude");

    private Vector2 _velocity;
    private int _x, _y;
    private Vector2 _last_move_dir;
    private Vector2 _dash_dir;
    private bool _dash_button_down;
    private float _dash_speed;

    private bool _can_dash;
    
    private enum State
    {
        Normal,
        Dashing
    }

    private State _state = State.Normal;

    private Action _apply_delay;
    private IEnumerator WaitDelayForConsecutiveDash(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        _can_dash = true;
    }
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rb2d = GetComponent<Rigidbody2D>();
        _can_dash = true;

        _apply_delay = dashDelay switch
        {
            0.0f => () => { },
            _ => () => StartCoroutine(WaitDelayForConsecutiveDash(dashDelay))
        };

    }

    private void OnEnable()
    {
        Stats.OnMovementSpeedMultiplierChanged += OnMovementSpeedMultiplierChangedCB;
    }

    private void OnDisable()
    {
        Stats.OnMovementSpeedMultiplierChanged -= OnMovementSpeedMultiplierChangedCB;
    }
    public void OnMovementSpeedMultiplierChangedCB(float speedMultiplier) => movementScalar = speedMultiplier;
    
    private void UpdateAnimation()
    {
        _animator.SetFloat(Horizontal, _velocity.x);
        _animator.SetFloat(Vertical, _velocity.y);
        _animator.SetFloat(Magnitude,  _velocity.magnitude);
    }

    private void GetKeyboardInput()
    {
        switch (_state)
        {
            case State.Normal:
                
                var w = Input.GetKey(KeyCode.W);
                var a = Input.GetKey(KeyCode.A);
                var s = Input.GetKey(KeyCode.S);
                var d = Input.GetKey(KeyCode.D);
                _x = (a ? -1 : 0) + (d ? 1 : 0);
                _y = (s ? -1 : 0) + (w ? 1 : 0);

                if (_x != 0 || _y != 0)
                {
                    // Kevork didn't like this
                    _last_move_dir = new Vector2(_x, _y).normalized;
                }
                
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (_can_dash)
                    {
                        _dash_dir = new Vector2(_x, _y).normalized;;
                        _dash_button_down = true;
                        _dash_speed = dashSpeed;
                        _state = State.Dashing;
                        _can_dash = false;
                        _apply_delay.Invoke();
                    }
                }
                break;
            case State.Dashing:
                
                break;
        }
    }

    private void MoveUsingTranslation()
    {
        _velocity = new Vector2(_x, _y).normalized * Time.fixedDeltaTime * movementScalar;
        transform.position += (Vector3) _velocity;
    }

    private void MoveUsingPhysics()
    {
        switch (_state)
        {
            case State.Normal:
                var move_dir = new Vector2(_x, _y).normalized;
                _velocity = move_dir * movementScalar;
                _rb2d.velocity = _velocity;
                break;
            
            case State.Dashing:
                float dash_speed_drop_multiplier = 3f;
                _dash_speed -= _dash_speed * dash_speed_drop_multiplier * Time.deltaTime;
                
                var dash_speed_minimum = movementScalar;
                if (_dash_speed < dash_speed_minimum)
                {
                    _state = State.Normal;
                }
                
                _rb2d.velocity = _dash_dir * _dash_speed; 
                break;
        }
        
        /*
        if (_dash_button_down)
        {
            Debug.Log($"move_dir = {move_dir}, dashAmount = {dashAmount} current pos = {transform.position} pos after dash = {(Vector2)transform.position + move_dir * dashAmount}");
            _rb2d.MovePosition((Vector2) transform.position + move_dir * dashAmount);
            _dash_button_down = false;
        }
        */
    }

    private void Update()
    {
        GetKeyboardInput();
        UpdateAnimation();
        if (_velocity.magnitude > 0) OnPlayerMovement?.Invoke();
    }

    private void FixedUpdate()
    {
        MoveUsingPhysics();
        // MoveUsingTranslation();
    } 
}
