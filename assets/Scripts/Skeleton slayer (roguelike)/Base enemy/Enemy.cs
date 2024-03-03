using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLU.SteeringBehaviors;

enum EnemyStates
{
    Wandering = 0,
    Chasing
}
// Gemaakt door Thijs Bosma
[RequireComponent(typeof(Steering))]
public abstract class Enemy : MonoBehaviour
{
    [Header("ShootingValues")]
    [SerializeField] protected GameObject _BulletPrefab;
    [SerializeField] protected Transform _BulletSpawnPoint;
    [SerializeField] private float _ShootCooldown;
    [SerializeField] protected int _BulletDamage;
    protected Animator _animator;
    protected bool _canShoot = true;
    protected Coroutine _coolDownCoroutine;

    [Header("EnemyVisuals")]
    [SerializeField] protected Transform _EnemyTransform;
    [SerializeField] private GameObject _EnemyModel;
    [SerializeField] protected Transform _OriginalTransform;

    [Header("AIVariables")]
    protected GameObject _Target;
    private List<IBehavior> _behaviors = new List<IBehavior>();
    private Steering _steering;
    private EnemyStates _enemyStates;

    private bool _isWandering = false;
    private bool _isChasing = false;

    [Header("DetectionSystem")]
    [SerializeField] private float _DetectionRadius;
    [SerializeField] private float _DetectionAngle;

    private void Awake()
    {
        Finish._enemyAmount.Add(this.gameObject);
    }
    private void Start()
    {
        _steering = GetComponent<Steering>();
        _animator= GetComponent<Animator>();
        _Target = FindObjectOfType<PlayerMovement>().gameObject;
        _enemyStates = EnemyStates.Wandering;
    }

    /// <summary>
    /// The attacking function for the enemy.
    /// </summary>
    /// <param name="collider"></param>
    public abstract void Attack();

    /// <summary>
    /// Switches the states for the enemy.
    /// </summary>
    protected void SwitchEnemyStates()
    {
        switch (_enemyStates)
        {
            //Sets the state to wandering
            case EnemyStates.Wandering:
                if (_isWandering)
                {
                    break;
                }
                _behaviors.Clear();

                Wandering();
                break;
            //Sets the state to changing
            case EnemyStates.Chasing:
                if (_isChasing)
                {
                    break;
                }

                _behaviors.Clear();

                Chasing();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Wandering function for the enemy
    /// </summary>
    private void Wandering()
    {
        _behaviors.Add(new Wander(_EnemyTransform));
        _behaviors.Add(new FollowWall() { m_offset = -1.0f, m_angle = -20.0f, m_scale = 0.5f });
        _behaviors.Add(new FollowWall() { m_offset = 0.0f, m_angle = 0.0f, m_scale = 1.0f });
        _behaviors.Add(new FollowWall() { m_offset = 1.0f, m_angle = 20.0f, m_scale = 0.5f });
        _behaviors.Add(new AvoidObstacle() { m_offset = -1.0f, m_angle = -20.0f, m_scale = 0.5f });
        _behaviors.Add(new AvoidObstacle() { m_offset = 0.0f, m_angle = 0.0f, m_scale = 1.0f });
        _behaviors.Add(new AvoidObstacle() { m_offset = 1.0f, m_angle = 20.0f, m_scale = 0.5f });
        _behaviors.Add(new AvoidWall() { m_offset = -1.0f, m_angle = -20.0f, m_scale = 0.5f });
        _behaviors.Add(new AvoidWall() { m_offset = 0.0f, m_angle = 0.0f, m_scale = 1.0f });
        _behaviors.Add(new AvoidWall() { m_offset = 1.0f, m_angle = 20.0f, m_scale = 0.5f });
        _steering.SetBehaviors(_behaviors);
        _animator.SetInteger("AnimationStateChange", 1);
        _isWandering = true;
    }

    /// <summary>
    /// Chasing function for the enemy
    /// </summary>
    private void Chasing()
    {
        _behaviors.Add(new Seek(_Target));
        _behaviors.Add(new FollowWall() { m_offset = -1.0f, m_angle = -20.0f, m_scale = 0.5f });
        _behaviors.Add(new FollowWall() { m_offset = 0.0f, m_angle = 0.0f, m_scale = 1.0f });
        _behaviors.Add(new FollowWall() { m_offset = 1.0f, m_angle = 20.0f, m_scale = 0.5f });
        _behaviors.Add(new AvoidObstacle() { m_offset = -1.0f, m_angle = -20.0f, m_scale = 0.5f });
        _behaviors.Add(new AvoidObstacle() { m_offset = 0.0f, m_angle = 0.0f, m_scale = 1.0f });
        _behaviors.Add(new AvoidObstacle() { m_offset = 1.0f, m_angle = 20.0f, m_scale = 0.5f });
        _behaviors.Add(new AvoidWall() { m_offset = -1.0f, m_angle = -20.0f, m_scale = 0.5f });
        _behaviors.Add(new AvoidWall() { m_offset = 0.0f, m_angle = 0.0f, m_scale = 1.0f });
        _behaviors.Add(new AvoidWall() { m_offset = 1.0f, m_angle = 20.0f, m_scale = 0.5f });
        _steering.SetBehaviors(_behaviors);
        _isChasing = true;
    }

    /// <summary>
    /// This function handles the cooldown for the shooting
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator GunCooldown()
    {
        yield return new WaitForSeconds(_ShootCooldown);
        _canShoot = true;
        _coolDownCoroutine = null;
    }

    /// <summary>
    /// Looks if the player is in the range of the detection radius
    /// </summary>
    /// <returns></returns>
    protected PlayerMovement LookForPlayer()
    {
        if(PlayerMovement._PlayerMovementInstance == null)
        {
            return null;
        }

        Vector3 enemyPosition = transform.position;
        Vector3 toPlayer = PlayerMovement._PlayerMovementInstance.transform.position - enemyPosition;
        toPlayer.y = 0;

        if(toPlayer.magnitude <= _DetectionRadius)
        {
            if(Vector3.Dot(toPlayer.normalized, transform.forward) > Mathf.Cos(_DetectionAngle * 0.5f * Mathf.Deg2Rad))
            {
                _enemyStates = EnemyStates.Chasing;
                return PlayerMovement._PlayerMovementInstance;
            }
        }
            
        return null;
    }


    private void OnValidate()
    {
        _ShootCooldown = Mathf.Max(_ShootCooldown, 1);
        _EnemyTransform = transform;
    }

    private void OnDestroy()
    {
        Finish._enemyAmount.Remove(this.gameObject);
    }

    /// <summary>
    /// Gets called in the animation in a animation event so the bow of the enemy is aimed towards the player
    /// </summary>
    public void RotationOffset()
    {
        _EnemyModel.transform.Rotate(0, 70, 0);
    }

    public void ResetRotation()
    {
        _EnemyModel.transform.rotation = _OriginalTransform.rotation;
    }

    //Draws the radius and angle of the detection sphere
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Color c = new Color(0.8f, 0, 0, 0.4f);
        UnityEditor.Handles.color = c;

        Vector3 rotatedForward = Quaternion.Euler(0, -_DetectionAngle * 0.5f, 0) * transform.forward;

        UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, rotatedForward, _DetectionAngle, _DetectionRadius);
    }

#endif
}