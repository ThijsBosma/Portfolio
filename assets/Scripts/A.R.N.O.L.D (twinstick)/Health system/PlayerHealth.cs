using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    [Header("Cheats")]
    [SerializeField] private bool _InfHealth;
    [SerializeField] private BarFill _healthBar;
    [SerializeField] private BarFill _shieldBar;

    private LoadNextScene _sceneLoader;

    private void Start()
    {
        _sceneLoader = FindObjectOfType<LoadNextScene>();
        _healthBar.SetMaxAmount(_MaxHealth);
        _shieldBar.SetMaxAmount(_MaxShield);
    }

    private void Update()
    {
        //if the InfHealth bool is activated
        if(_InfHealth)
        {
            //Maxhealth becomes infinite
            _MaxHealth = Mathf.Infinity;
            _CurrentHealth = Mathf.Infinity;
        }

        if(_CurrentHealth <= 0)
            Kill();
    }

    //Adds armor when the function is called
    public override void AddArmor(float armorAmount)
    {
        base.AddArmor(armorAmount);
        
        _shieldBar.UpdateFillAmount(_currentShield);
    }

    //Adds health when the function is called
    public override void AddHealth(float healAmount)
    {
        base.AddHealth(healAmount);
        
        _healthBar.UpdateFillAmount(_CurrentHealth);
    }

    //Deals damage to things when the function is called
    public override void DealDamage(float damageAmount)
    {
        base.DealDamage(damageAmount);
        
        _shieldBar.UpdateFillAmount(_currentShield);
        _healthBar.UpdateFillAmount(_CurrentHealth);
    }

    //Kills the object when the function is called
    public override void Kill()
    {
        base.Kill();
        
        _shieldBar.SetMaxAmount(_currentShield);
        _healthBar.SetMaxAmount(_CurrentHealth);

        _sceneLoader.LoadScene("Death Screen");
    }
}
