using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpiderController : MonoBehaviour
{
    [SerializeField] private float movingSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private List<Transform> legIkTargets;
    [SerializeField] private List<Transform> legIkSafezones;
    [SerializeField] private float ikSafezoneRadius;
    [SerializeField] private float legTimingOffset;
    [SerializeField] private float velocityMultiplier;
    [SerializeField] private AnimationCurve legHeightAnimationCurve;
    private List<Vector3> legIkPositions;
    private Vector2 _movement;
    private float previousMoveTime;
    private int[] movingPattern = { 4, 2, 6, 0, 3, 5, 1, 7 };//{0,1,2,3,4,5,6,7,8};
    private int currentPatternIndex = 0;
    private Vector3 lastTransformPos;
    private Vector3 velocity;

    private void Start()
    {
        _movement = new Vector3();
        legIkPositions = new List<Vector3>();
        for (int i = 0; i < legIkTargets.Count; i++)
        {
            legIkPositions.Add(legIkTargets[i].position);
        }

        previousMoveTime = Time.time;
        lastTransformPos = transform.position;
    }

    void Update()
    {
        _movement.x = Input.GetAxis("Horizontal");
        _movement.y = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        velocity = transform.position - lastTransformPos;
        
        transform.RotateAround(transform.up,(rotationSpeed*_movement.x*Time.fixedDeltaTime));
        transform.position += transform.forward * (_movement.y * movingSpeed * Time.fixedDeltaTime);
        UpdateLegs();
        int legToMove = GetLegIndexToMove();
        //pour chaque patte
        //
        lastTransformPos = transform.position;
    }

    private int GetLegIndexToMove()
    {
        float maxDist = ikSafezoneRadius;
        int id = -1;
        for (int i = 0; i < legIkTargets.Count; i++)
        {
            float dist =  Vector3.ProjectOnPlane(legIkSafezones[i].position + velocity * velocityMultiplier - legIkPositions[i], transform.up).magnitude;
            if (dist > maxDist)
            {
                maxDist = dist;
                id = i;
            }
        }
        return id;
    }

    private void UpdateLegs()
    {
        for (int i = 0; i < legIkTargets.Count; i++)
        {
            if (movingPattern[currentPatternIndex] == i)
            {
                float elapsed = Time.time - previousMoveTime;
                if (elapsed >= legTimingOffset)
                {
                    Vector3 safePos = legIkSafezones[i].position;
                    Vector3 targetPos = legIkTargets[i].position;
                    if ((safePos - targetPos).magnitude > ikSafezoneRadius)
                    {
                        previousMoveTime = Time.time;
                        currentPatternIndex = (currentPatternIndex + 1) % 8;
                        legIkPositions[i] = safePos + velocity.normalized * (ikSafezoneRadius * 2);
                    }
                }    
            }
            legIkTargets[i].position = legIkPositions[i];//locking the bone to the proper position
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
