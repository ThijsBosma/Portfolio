using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using UnityEngine;

public class GhostRecorder : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private float _IntervalBetweenSaves;
    [SerializeField] private Car _Car;
    private Finish _finish;
    
    private Ghost _ghost = new Ghost();
    private GameManager _gameManager;

    [SerializeField] private Transform _PlayerTransform;

    private float _ghostTimer;


    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _finish = FindObjectOfType<Finish>();
    }

    void Update()
    {
        RecordPositions();
    }

    private void RecordPositions()
    {
        _ghostTimer += Time.deltaTime;
        //if the timer is higher then the time between saves
        if (_ghostTimer >= _IntervalBetweenSaves)
        {
            //put back the ghost timer to zero
            _ghostTimer = 0;
            //Add the timestamp position and rotation to their respective list
            _ghost._TimeStamps.Add(_gameManager._Timer);
            _ghost._SavedPositions.Add(_PlayerTransform.position);
            _ghost._SavedRotations.Add(_PlayerTransform.rotation);
        }
    }

    /// <summary>
    /// Saves the data to json.
    /// </summary>
    public void SaveRecording()
    {
        string recordedPlayerMovements = JsonUtility.ToJson(_ghost);
        string filePath = Application.persistentDataPath + "/GhostData.json";
        
        Debug.Log(filePath);

        System.IO.File.WriteAllText(filePath, recordedPlayerMovements);
        Debug.Log("Files Saved");
    }
}
