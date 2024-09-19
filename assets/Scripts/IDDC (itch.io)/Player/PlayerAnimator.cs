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

        //Check to see if the player is moving and not picking anything up
        if (_movementValue.magnitude > 0.5f && !_isPickingUp)
        {   
            //Set the player animator int to 1 which is equal to the walking/running state.
            _PlayerAnimator.speed = 1;
            _PlayerAnimator.SetInteger("PlayerState", 1);
        }
        //if the player is not moving and not picking up
        else if (_movementValue.magnitude < 0.5f && !_isPickingUp)
        {
            //Set the integer to 0 which is equal to idle.
            _PlayerAnimator.SetInteger("PlayerState", 0);
        }

        //If the player is climbing.
        if (_controller._IsClimbing)
        {
            //Set the integer to 3 which is equal to the climbing animation
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
