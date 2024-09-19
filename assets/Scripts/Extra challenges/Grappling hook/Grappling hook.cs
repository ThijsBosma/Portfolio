using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grapplinghook : MonoBehaviour
{
    public enum GrappleState
    {
        IsShooting = 0,
        ShootLine,
        MovePlayer
    }

    [Header("Components")]
    [SerializeField] private Transform _PlayerTransform;
    [SerializeField] private Transform _GrappleTransform;
    [SerializeField] private LayerMask _IsGrapplable;
    [SerializeField] private LineRenderer _GrappleVisual;
    [SerializeField] private PlayerController _PlayerController;
    private PauseMenu _pauseMenu;
    private RaycastHit2D _GrappleHit;
    private Vector2 _CurrentGrappleHookPosition;

    [Header("WeaponVariables")]
    [SerializeField] private float _MaxGrappleDistance;
    [SerializeField] private float _GrappleTime; //How long it takes to grapple to the given position
    private float _ThreshHold = 0.5f;
    public GrappleState _state;

    private Vector2 mousePosition;

    private void Start()
    {
        _state = GrappleState.IsShooting;
        _GrappleVisual.enabled = false;
        _pauseMenu = FindObjectOfType<PauseMenu>(); 
    }

    void Update()
    {
        ShootGrapplingHook();    
    }


    private void ShootGrapplingHook()
    {
        mousePosition = _PlayerController.GetLookDirection();

        _CurrentGrappleHookPosition.x = _GrappleTransform.position.x;
        _CurrentGrappleHookPosition.y = _GrappleTransform.position.y;

        //Check which state the grappling hook is in.
        switch(_state)
        {
            //if the state is shooting
            case GrappleState.IsShooting:
                if (!_pauseMenu._IsPaused)
                {
                    //Get the shooting input
                    if (_PlayerController.GetIsShooting())
                    {
                        //Shoot a raycast to see where the player shot
                        _GrappleHit = Physics2D.Raycast(_PlayerTransform.position, (mousePosition - new Vector2(_PlayerTransform.position.x, _PlayerTransform.position.y)).normalized, Vector2.Distance(_PlayerTransform.position, mousePosition), _IsGrapplable);
                        
                        //If it hits something
                        if (_GrappleHit)
                        {
                            //Put the player on static.
                            _PlayerController.body.bodyType = RigidbodyType2D.Static;
                            //Set the state to shoot the line
                            _state = GrappleState.ShootLine;
                        }
                    }
                }
                break;
                //if the state is ShootLine
            case GrappleState.ShootLine:
                _GrappleVisual.enabled = true;
                //Set the line renderer position 0 to where the player is
                _GrappleVisual.SetPosition(0, _PlayerTransform.position);
                //set the line renderer position to where the player clicked
                _GrappleVisual.SetPosition(1, _GrappleHit.point);

                //Change the state
                _state = GrappleState.MovePlayer;
                break;
                //if the state is in movePlayer
            case GrappleState.MovePlayer:
                
                //Lerp the playerposition from where he is standing to the spot where the grapple hit.
                _PlayerTransform.position = Vector2.Lerp(_PlayerTransform.position, _GrappleHit.point, _GrappleTime * Time.deltaTime);
                //Set the position of the visual to the playerposition
                _GrappleVisual.SetPosition(0, _PlayerTransform.position);

                //Check to see if the player has finished the grappling
                if (Vector2.Distance(_PlayerTransform.position, _GrappleHit.point) <= _ThreshHold)
                {
                    //Disable the visual
                    _GrappleVisual.enabled = false;
                    //Reset the state to default
                    _state = GrappleState.IsShooting;
                    //Put the rigidbody back to dynamic.
                    _PlayerController.body.bodyType = RigidbodyType2D.Dynamic;

                }
                break;
            default:
                break;
        }
    }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_PlayerTransform.position, mousePosition);
    }
}
