using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Ghost
{
    public List<Vector3> _SavedPositions = new List<Vector3>();
    public List<Quaternion> _SavedRotations = new List<Quaternion>();
    public List<float> _TimeStamps = new List<float>();
    
    public void ClearData()
    {
        _SavedPositions.Clear();
        _SavedRotations.Clear();
        _TimeStamps.Clear();
    }
}
