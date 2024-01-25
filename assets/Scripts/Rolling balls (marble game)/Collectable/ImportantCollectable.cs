using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportantCollectable : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameManager gameManager;

    private Vector3 startingPosition;

    private void Start()
    {
        startingPosition = playerMovement.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        gameManager.AddGoldBarScore();
        gameManager.goldBars.Add(gameObject);
        Invoke("ResetPlayerPosition", 1);
        Destroy(gameObject, 1.2f);
    }

    //Puts player back on its original position
    private void ResetPlayerPosition()
    {
        playerMovement.transform.position = startingPosition;
    }
}
