using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform _SpawnPoint;
    private GameManager _gameManager;
    private Respawn _respawn;

    [Header("CheckpointNecessities")]
    public int _CheckpointID; //Which checkpoint it is in the track

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }
    private void Update()
    {
        //if the current checkpoint is higher then or equal to the all the checkpoints
        if (_gameManager._CurrentCheckpoint >= _gameManager._Checkpoints.Length)
        {
            //Add a lap to the currentlap counter
            _gameManager._CurrentLap += 1;
            //Put the CurrentCheckpoint back to 0 so you can restart
            _gameManager._CurrentCheckpoint = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Car>())
        {
            //If the respawn script is not filled
            if (_respawn == null)
            {
                //Find the respawn script
                _respawn = FindObjectOfType<Respawn>();
            }

            //If the CurrentCheckpoint is equal to the checkpointID
            if (_gameManager._CurrentCheckpoint == _CheckpointID)
            {
                //Add 1 to the _CurrentCheckpoint int
                _gameManager._CurrentCheckpoint += 1;
                //Sets the position to the position of the checkpoint
                _respawn._RespawnPosition = _SpawnPoint.position;
                //Sets the rotation to the rotation of the checkpoint
                _respawn._RespawnRotation = _SpawnPoint.rotation;
            }
        }
    }
}
