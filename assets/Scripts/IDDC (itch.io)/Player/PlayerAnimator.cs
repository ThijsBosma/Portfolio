using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAnimator : InputHandler
{
    [SerializeField] private Animator _PlayerAnimator;
    [SerializeField] private Animation _ClimbingAnimataion;
    private bool _isPickingUp;

    private Vector2 _movementValue;
    private ThirdPersonController _controller;

    private void Start()
    {
        _controller = FindObjectOfType<ThirdPersonController>();

        _PlayerAnimator.SetInteger("PlayerState", 0);
    }

    private void Update()
    {
        if (!GameManager._Instance._enableMove)
            return;

        _movementValue = _Move.ReadValue<Vector2>();

        if (_movementValue.magnitude > 0.5f && !_isPickingUp)
        {
            _PlayerAnimator.speed = 1;
            _PlayerAnimator.SetInteger("PlayerState", 1);
        }
        else if (_movementValue.magnitude < 0.5f && !_isPickingUp)
        {
            _PlayerAnimator.SetInteger("PlayerState", 0);
        }

        if (_controller._IsClimbing)
        {
            _PlayerAnimator.SetInteger("PlayerState", 3);
            if (_movementValue.y > 0)
                _PlayerAnimator.speed = 1;
            else if(_movementValue.y < 0)
            {
                //_PlayerAnimator.recorderMode = AnimatorRecorderMode.Playback;
                //_PlayerAnimator.speed = -1;
            }
            else if (_movementValue == Vector2.zero)
                _PlayerAnimator.speed = 0;
        }

        //if(_Pickup.WasPressedThisFrame())
        //{
        //    _isPickingUp = true;
        //    _PlayerAnimator.SetInteger("PlayerState", 2);
        //}
    }
}
