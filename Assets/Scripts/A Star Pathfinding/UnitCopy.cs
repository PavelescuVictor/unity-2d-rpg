﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCopy : MonoBehaviour
{
    private GameObject target;
    public float speed;
    private Vector2[] path;
    private int targetIndex;
    public float chaseRadius;
    public float attackRadius;
    

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("MainCharacter");
        StartCoroutine(RefreshPath());
    }

    IEnumerator RefreshPath()
    {
        Vector2 targetPositionOld = (Vector2)target.transform.position + Vector2.up;
        while(true)
        {
            if (targetPositionOld != (Vector2) target.transform.position)
            {
                targetPositionOld = (Vector2) target.transform.position;

                path = Pathfinding.RequestPath(transform.position, target.transform.position);
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }

            yield return new WaitForSeconds(.25f);
        }
    }

    IEnumerator FollowPath()
    {
        if (path.Length > 0)
        {
            targetIndex = 0;
            Vector2 currentWaypoint = path[0];

            while (true)
            {
                if ((Vector2)transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        yield break;
                    }

                    currentWaypoint = path[targetIndex];
                }

                if (Vector3.Distance(target.transform.position, transform.position) <= chaseRadius &&
                    Vector3.Distance(target.transform.position, transform.position) > attackRadius)
                {
                    transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                }
                yield return null;
            } 
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawCube((Vector3)path[i], Vector3.one * .5f);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i-1], path[i]);
                }
            }
        }
    }
}
