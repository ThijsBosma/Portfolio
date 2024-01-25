using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostReplayer : MonoBehaviour
{
    [SerializeField] private Transform _OriginalTransform;
    private Ghost _ghost = new Ghost();
    private GameManager _gameManager;   

    private Car _car;

    [HideInInspector] public float _TimeValue;
    [HideInInspector] public int _Index1;
    [HideInInspector] public int _Index2;

    private void Start()
    {
        _car = FindObjectOfType<Car>();
        _gameManager = FindObjectOfType<GameManager>();

        string filePath = Application.persistentDataPath + "/GhostData.json";
        
        if (System.IO.File.Exists(filePath))
        {
            string recordedData = System.IO.File.ReadAllText(filePath);
            
            _ghost = JsonUtility.FromJson<Ghost>(recordedData);
            Debug.Log("Loaded");
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (_car._CanStart)
        {
            if (!_gameManager._PauseAssets.activeInHierarchy)
            {
                _TimeValue += Time.unscaledDeltaTime;
                GetIndex();
                SetTransform();
            }
        }
        else if(!_car._CanStart)
        {
            gameObject.transform.position = _OriginalTransform.position;
            gameObject.transform.rotation = _OriginalTransform.rotation;
        }
    }

    private void GetIndex()
    {
        for (int i = 0; i < _ghost._TimeStamps.Count - 2; i++)
        {
            if (_ghost._TimeStamps[i] == _TimeValue)
            {
                _Index1 = i;
                _Index2 = i;
                return;
            }
            else if (_ghost._TimeStamps[i] < _TimeValue && _TimeValue < _ghost._TimeStamps[i + 1])
            {
                _Index1 = i;
                _Index2 = i + 1;
                return;
            }
        }

        _Index1 = _ghost._TimeStamps.Count - 1;
        _Index2 = _ghost._TimeStamps.Count - 1;
    }

    private void SetTransform()
    {
        if (_Index1 == _Index2)
        {
            transform.position = _ghost._SavedPositions[_Index1];
            transform.rotation = _ghost._SavedRotations[_Index1];
        }
        else
        {
            float interpolationFactor = (_TimeValue - _ghost._TimeStamps[_Index1]) / (_ghost._TimeStamps[_Index2] - _ghost._TimeStamps[_Index1]);
            transform.position = Vector3.Lerp(_ghost._SavedPositions[_Index1], _ghost._SavedPositions[_Index2], interpolationFactor);
            transform.rotation = Quaternion.Slerp(_ghost._SavedRotations[_Index1], _ghost._SavedRotations[_Index2], interpolationFactor);
        }
    }
}
