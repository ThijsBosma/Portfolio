using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{
    [SerializeField] protected float _MaxHealth;
    [SerializeField] protected float _MaxShield;
    
    protected float _CurrentHealth;
    protected float _currentShield;

    protected GameManager _gameManager;

    private void Awake()
    {
        _CurrentHealth = _MaxHealth;
        _currentShield = _MaxShield;
        _gameManager = FindObjectOfType<GameManager>();
    }
    
    public virtual void Kill() 
    {
        Destroy(gameObject);
    }

    public virtual void DealDamage(float damageAmount)
    {
        if (_currentShield != 0)
        {
            _currentShield -= damageAmount;
            _currentShield = Mathf.Clamp(_currentShield, 0, _MaxShield);
        }

        else
        {
            _CurrentHealth -= damageAmount;
            _CurrentHealth = Mathf.Clamp(_CurrentHealth, 0, _MaxHealth);
        }
    }

    public virtual void AddHealth(float healAmount)
    {
        _CurrentHealth += healAmount;
    }

    public virtual void AddArmor(float armorAmount)
    {
        _currentShield += armorAmount;
    }


}
