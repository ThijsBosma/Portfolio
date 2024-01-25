using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealths : Health
{
    void Update()
    {
        if (_CurrentHealth <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (_gameManager._AliveEnemies.Count == 1)
        {
            Instantiate(_gameManager._KeycardPrefab, transform.position, Quaternion.identity);
        }
        _gameManager._AliveEnemies.Remove(transform.parent.gameObject);
    }
}
