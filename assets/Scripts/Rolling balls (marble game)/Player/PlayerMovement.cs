using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Walk")]
    [SerializeField] private float _MoveSpeed;
    [SerializeField] private float _CurrentDrag;
    private float _originalDrag; 
    private Vector3 _movementInputs;

    [Header("Jump")]
    [SerializeField] private float _JumpForce;
    [SerializeField] private LayerMask _JumpableGround;
    [SerializeField] private float _RaycastOffset;
    private SphereCollider _SphereCollision;

    [Header("Camera")]
    [SerializeField] private Transform _Camera;

    [Header("Audio")]
    [SerializeField] private AudioSource _PlayerAudio;
    [SerializeField] private AudioClip _JumpAudio;

    private Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        _SphereCollision = GetComponent<SphereCollider>();
    }

    void Update()
    {
        PlayerWalking();

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
        }

        if(Input.GetKey(KeyCode.S) || Input.GetButton("Brake"))
        {
            rigidBody.drag = _CurrentDrag;
        }
        else
        {
            rigidBody.drag = _originalDrag;
        }
    }

    private void PlayerWalking()
    {
        //Horizontal input van de input manager (A en D)
        float x = Input.GetAxis("Horizontal");
        //Vertical input van de input managaer (W en S)
        float z = Input.GetAxis("Vertical");

        _movementInputs = new Vector3(x, 0, z);
        //Ik voeg force toe aan de RB met de inputs vector en dat doe ik keer de moveSpeed en * deltaTime
        rigidBody.AddForce(_Camera.rotation * _movementInputs * _MoveSpeed * Time.deltaTime, ForceMode.Impulse);
    }

    private void Jump()
    {
        //Ik voeg force toe omhoog en dat doe ik keer de jumpForce
        rigidBody.AddForce(Vector3.up * _JumpForce, ForceMode.Impulse);
        _PlayerAudio.PlayOneShot(_JumpAudio);
    }

    private bool IsGrounded()
    {
        //Ik stuur een lijn naar beneden die checkt of de layermask gelijk is aan de ground layer
        return Physics.Raycast(transform.position, Vector3.down, _SphereCollision.radius + _RaycastOffset, _JumpableGround);
    }

    public void ResetVelocity()
    {
        rigidBody.velocity = Vector3.zero;
    }
}
