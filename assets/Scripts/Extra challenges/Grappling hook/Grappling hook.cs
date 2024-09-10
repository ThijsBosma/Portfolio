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
        mousePosition = _PlayerController.GetLookDirection();

        _CurrentGrappleHookPosition.x = _GrappleTransform.position.x;
        _CurrentGrappleHookPosition.y = _GrappleTransform.position.y;

        switch(_state)
        {
            case GrappleState.IsShooting:
                if (!_pauseMenu._IsPaused)
                {
                    if (_PlayerController.GetIsShooting())
                    {
                        _GrappleHit = Physics2D.Raycast(_PlayerTransform.position, (mousePosition - new Vector2(_PlayerTransform.position.x, _PlayerTransform.position.y)).normalized, Vector2.Distance(_PlayerTransform.position, mousePosition), _IsGrapplable);

                        if (_GrappleHit)
                        {
                            _PlayerController.body.bodyType = RigidbodyType2D.Static;
                            _state = GrappleState.ShootLine;
                        }
                    }
                }
                break;
            case GrappleState.ShootLine:
                //Lerp GrappleLine to the hit position
                _GrappleVisual.enabled = true;
                _GrappleVisual.SetPosition(0, _PlayerTransform.position);
                _GrappleVisual.SetPosition(1, _GrappleHit.point);

                _state = GrappleState.MovePlayer;
                break;
            case GrappleState.MovePlayer:

                _PlayerTransform.position = Vector2.Lerp(_PlayerTransform.position, _GrappleHit.point, _GrappleTime * Time.deltaTime);
                _GrappleVisual.SetPosition(0, _PlayerTransform.position);

                //If the lerp is done change the state
                if (Vector2.Distance(_PlayerTransform.position, _GrappleHit.point) <= _ThreshHold)
                {
                    _GrappleVisual.enabled = false;
                    _state = GrappleState.IsShooting;
                    _PlayerController.body.bodyType = RigidbodyType2D.Dynamic;

                }
                break;
            default:
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_PlayerTransform.position, mousePosition);
    }
}
