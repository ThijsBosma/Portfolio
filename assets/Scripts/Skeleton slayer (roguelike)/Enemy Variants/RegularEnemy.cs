using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularEnemy : Enemy
{
    private void Update()
    {
        SwitchEnemyStates();

        if (!_canShoot && _coolDownCoroutine == null)
        {
            _coolDownCoroutine = StartCoroutine(GunCooldown());
        }
    }

    private void FixedUpdate()
    {
        if (LookForPlayer())
        {
            _animator.SetInteger("AnimationStateChange", 2);
        }
        else
        {
            _animator.SetInteger("AnimationStateChange", 1);
        }
    }

    public override void Attack()
    {
        GameObject bullet = Instantiate(original: _BulletPrefab, position: _BulletSpawnPoint.position, rotation: _BulletSpawnPoint.rotation);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript._Damage = _BulletDamage;
        
        bulletScript.GetComponent<Rigidbody>().AddForce((_Target.transform.position - _BulletSpawnPoint.position) * bulletScript._MoveSpeed, ForceMode.Impulse);

        _canShoot = false;
    }

    /// <summary>
    /// This function gets called in the animator to perform the attack
    /// </summary>
    public void ShootArrow()
    {
        if (LookForPlayer() && _canShoot)
        {
            Attack();
        }
    }
}
