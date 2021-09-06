using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class EnterPortalBehavior : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float proceedToNextWaypointRadius = 0.1f;
    private GameObject _targetTransformGameObject;
    private Transform _targetTransform;
    private GameObject _navigationSystemGameObject;
    private NavigationSystem.NavigationSystem _navigationSystem;
    private List<Vector2> _shortestPath;
    private Vector2 _currentWaypoint;

    private void Awake()
    {
        _navigationSystemGameObject = GameObject.FindWithTag("NavigationSystem");
        _navigationSystem = _navigationSystemGameObject.GetComponent<NavigationSystem.NavigationSystem>();

        _targetTransformGameObject = GameObject.FindWithTag("Target");
        _targetTransform = _targetTransformGameObject.transform;
    }

    /* Like the Awake function, Start is called exactly once in the lifetime of the script. However, Awake is called when the script object is initialised, regardless of whether or not the script is enabled. Start may not be called on the same frame as Awake if the script is not enabled at initialisation time. If variable initialisation can be deferred until the script is enabled, use Start (lazy loading). */
    void Start()
    {
        var startingPosition = _navigationSystem.GetNearestNeighbor(transform.position);
        var endPosition = _navigationSystem.GetNearestNeighbor(_targetTransform.position);
        _shortestPath = new List<Vector2>(_navigationSystem.GetShortestPath(startingPosition, endPosition));
        _currentWaypoint = _shortestPath.First();
    }

    void Update()
    {
        if (_currentWaypoint == Vector2.zero) return;

        Vector2 currentPosition = transform.position;

        SetNexWaypoint(currentPosition);
        MoveEnemy(currentPosition);
    }

    private void SetNexWaypoint(Vector2 currentPosition)
    {
        Vector2 distance = _currentWaypoint - currentPosition;
        float distanceSqr = distance.sqrMagnitude;

        if (distanceSqr > proceedToNextWaypointRadius * proceedToNextWaypointRadius) return;

        var curIdx = _shortestPath.IndexOf(_currentWaypoint);
        curIdx++;

        if (curIdx >= _shortestPath.Count)
        {
            _currentWaypoint = Vector2.zero;
            transform.Translate(Vector3.zero, Space.World);
        }
        else
        {
            _currentWaypoint = _shortestPath.ElementAt(curIdx);
        }
    }

    private void MoveEnemy(Vector2 currentPosition)
    {
        if (_currentWaypoint == Vector2.zero) return;

        Vector2 distance = _currentWaypoint - currentPosition;
        Vector2 direction = distance.normalized;

        Vector2 velocity = direction * speed * Time.deltaTime;
        transform.Translate(velocity, Space.World);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.05f);

        _navigationSystemGameObject = GameObject.FindWithTag("NavigationSystem");
        _navigationSystem = _navigationSystemGameObject.GetComponent<NavigationSystem.NavigationSystem>();

        _targetTransformGameObject = GameObject.FindWithTag("Target");
        _targetTransform = _targetTransformGameObject.transform;

        var startingPosition = _navigationSystem.GetNearestNeighbor(transform.position);
        var endPosition = _navigationSystem.GetNearestNeighbor(_targetTransform.position);
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(startingPosition, 0.05f);
        Gizmos.DrawSphere(endPosition, 0.05f);

        var shortestPath = _navigationSystem.GetShortestPath(startingPosition, endPosition);

        Gizmos.color = Color.red;
        foreach (var path in shortestPath)
        {
            Gizmos.DrawSphere(path, 0.05f);
        }
    }
}