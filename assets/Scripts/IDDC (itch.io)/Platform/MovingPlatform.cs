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

        while (time < 1)
        {
            time += Time.deltaTime / _Time;

            float t = _Curve.Evaluate(time);
            transform.position = Vector3.Lerp(_startPostion, _endPostion, t);
            yield return null;
        }

        yield return new WaitForSeconds(_TimeBeforeMoving);
        _currentIndex += 1;

        if (IsIndexOutOfBounds())
        {
            _currentIndex = _Waypoints.Length;
            _endPostion = _Waypoints[0].position;
            _startPostion = _Waypoints[_currentIndex - 1].position;
            _currentIndex = _Waypoints.Length - 1;
        }
        else
        {
            _endPostion = _Waypoints[_currentIndex + 1].position;
            _startPostion = _Waypoints[_currentIndex].position;
        }

        if (_isLooping == true)
        {
            _currentIndex = 0;
            _endPostion = _Waypoints[_currentIndex + 1].position;
            _startPostion = _Waypoints[_currentIndex].position;
            _isLooping = false;
        }

        _coroutine = null;
    }
    private void OnTriggerEnter(Collider other)
    {
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
