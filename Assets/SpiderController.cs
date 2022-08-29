using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpiderController : MonoBehaviour
{
    [SerializeField] private float movingSpeed;
    [SerializeField] private List<Transform> legIkTargets;
    [SerializeField] private List<Transform> legIkSafezones;
    [SerializeField] private float ikSafezoneRadius;
    private List<Vector3> legIkPositions;
    private Vector3 _movement;

    private void Start()
    {
        _movement = new Vector3();
        legIkPositions = new List<Vector3>();
        for (int i = 0; i < legIkTargets.Count; i++)
        {
            legIkPositions.Add(legIkTargets[i].position);
        }
    }

    void Update()
    {
        _movement.x = Input.GetAxis("Horizontal");
        _movement.z = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        //TODO: fix the mesh to fix y axis rotation of 180 degrees
        transform.position += -_movement * (movingSpeed * Time.fixedDeltaTime);
        UpdateLegs();
    }

    private void UpdateLegs()
    {
        for (int i = 0; i < legIkTargets.Count; i++)
        {
            Vector3 safePos = legIkSafezones[i].position;
            Vector3 targetPos = legIkTargets[i].position;
            if ((safePos - targetPos).magnitude > ikSafezoneRadius)
            {
                legIkPositions[i] = safePos + -_movement.normalized * ikSafezoneRadius;
            }
            legIkTargets[i].position = legIkPositions[i];
        }
    }

    private void OnDrawGizmos()
    {
        //Drawing safe zones
        Gizmos.color = Color.yellow;
        foreach (var safezone in legIkSafezones)
        {
            Gizmos.DrawWireSphere(safezone.position,ikSafezoneRadius);
        }
        Gizmos.color = Color.green;
        foreach (var target in legIkTargets)
        {
            Gizmos.DrawSphere(target.position,0.1f);
        }
    }
}
