using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class KeycardPickup : MonoBehaviour
{
    [Header("UI")]
    public Image _keycardImage;

    [HideInInspector] public bool _KeyCardCollected;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<MovementPlayer>() != null)
        {
            _KeyCardCollected = true;
            Destroy(gameObject);
        }
    }
}
