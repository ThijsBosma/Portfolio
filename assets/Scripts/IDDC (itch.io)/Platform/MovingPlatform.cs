using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform[] _Waypoints;

    [SerializeField, Tooltip("Time for a lerp in seconds")] private float _Time;

    [SerializeField] private AnimationCurve _Curve;
    private int _currentIndex;

    private Vector3 _startPostion;
    private Vector3 _endPostion;

    private bool _isLooping;

    private Coroutine _coroutine;

    [SerializeField, Tooltip("In seconds")] private float _TimeBeforeMoving;

    private void Start()
    {
        _startPostion = _Waypoints[0].position;
        _endPostion = _Waypoints[1].position;
    }

    private void FixedUpdate()
    {
        if (_coroutine == null)
        {
            _coroutine = StartCoroutine(MovePlatform());
        }
    }

    private bool IsIndexOutOfBounds()
    {
        if (_currentIndex + 1 >= _Waypoints.Length)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator MovePlatform()
    {
        if (_currentIndex == _Waypoints.Length - 1)
        {
            _isLooping = true;
        }

        float time = 0;

        //While time is lower then 1
        while (time < 1)
        {
            time += Time.deltaTime / _Time;
            //set the animation curve time
            float t = _Curve.Evaluate(time);
            //Lerp between the start and endposition.
            transform.position = Vector3.Lerp(_startPostion, _endPostion, t);
            yield return null;
        }

        yield return new WaitForSeconds(_TimeBeforeMoving);
        _currentIndex += 1;
        //If the index is out of bounds
        if (IsIndexOutOfBounds())
        {
            //the currentindex is equal to the final point in the array
            _currentIndex = _Waypoints.Length;
            //The endposition is equal to the first point in the array
            _endPostion = _Waypoints[0].position;
            //The start position is equal to waypoints currentindex -1 position
            _startPostion = _Waypoints[_currentIndex - 1].position;
            //Current index is equal to the final point of the array - 1.
            _currentIndex = _Waypoints.Length - 1;
        }
        //If it is not out of bounds
        else
        {
            //the endposition gets put forward 1 in the array
            _endPostion = _Waypoints[_currentIndex + 1].position;
            //the startposition gets put at the current index;
            _startPostion = _Waypoints[_currentIndex].position;
        }
        //If the platform is looping
        if (_isLooping == true)
        {
            //Current index is equal to 0
            _currentIndex = 0;
            //the endposition is equal to the current index + 1.
            _endPostion = _Waypoints[_currentIndex + 1].position;
            //The  startposition is equal to the current index.
            _startPostion = _Waypoints[_currentIndex].position;
            //Is no longer looping.
            _isLooping = false;
        }

        _coroutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the player or box gets put on the platform set the parent to the platform
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }

        if (other.gameObject.CompareTag("Box"))
        {
            other.transform.SetParent(transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
}
