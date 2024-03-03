using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class MovementPlayer : MonoBehaviour
{
    [Header("Dash")]
    [SerializeField] private float _dashPower;
    [SerializeField] private float _dashCooldown;
    [SerializeField] private float _dashDuration;
    private bool _isDashing;

    [Header("References")]
    private CharacterController _characterController;

    private IEnumerator Dash()
    {
        Vector3 dashDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        float startTime = Time.time;
        //Puts is dashing to true
        _isDashing = true;

        //While the startTime is lower then Time.time + dashDuration
        while(Time.time < startTime + _dashDuration)
        {
            //Move the player from the _movementInput variable * dashPower
            _characterController.Move(dashDirection * _dashPower * Time.deltaTime);
            yield return null;
        }
        //Wait for the cooldown
        yield return new WaitForSeconds(_dashCooldown);
        //You are no longer dashing
        _isDashing = false;
    }
}
