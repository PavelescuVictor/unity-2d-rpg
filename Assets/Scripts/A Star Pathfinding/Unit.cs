using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Unit: MonoBehaviour
{
    private GameObject target;
    public float speed;
    private Vector2[] path;
    private int targetIndex;
    public float chaseRadius;
    public float attackRadius;
    public float unitPositionOffsetY;
    public float targetPositionOffsetY;
    private Vector2 targetPosition;


    void Start()
    {
        target = GameObject.FindGameObjectWithTag("MainCharacter");
        targetPosition = new Vector2(target.transform.position.x,target.transform.position.y - targetPositionOffsetY);
        StartCoroutine(RefreshPath());
    }

    void Update()
    {
        
        target = GameObject.FindGameObjectWithTag("MainCharacter");
        targetPosition = new Vector2(target.transform.position.x, target.transform.position.y - targetPositionOffsetY);
    }

    IEnumerator RefreshPath()
    {
        Vector2 targetPositionOld = targetPosition + Vector2.up;
        while(true)
        {
            if (targetPositionOld != targetPosition)
            {
                targetPositionOld = targetPosition;

                path = Pathfinding.RequestPath(
                    new Vector2(transform.position.x, transform.position.y - unitPositionOffsetY),
                    targetPosition);
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
                if (new Vector2(transform.position.x, transform.position.y - unitPositionOffsetY) == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        yield break;
                    }

                    currentWaypoint = path[targetIndex];
                }

                if (Vector3.Distance(targetPosition, transform.position) <= chaseRadius &&
                    Vector3.Distance(targetPosition, transform.position) > attackRadius)
                {
                    if (this.GetComponent<Enemy>().currentState == Enemy.EnemyState.walk)
                    {
                        transform.position = Vector2.MoveTowards(
                            transform.position,
                            new Vector2( currentWaypoint.x, currentWaypoint.y + unitPositionOffsetY),
                            speed * Time.deltaTime);
                    }
                }
                yield return null;
            } 
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube((Vector3) targetPosition, Vector3.one * .3f);
            Gizmos.DrawCube((Vector3) new Vector2(transform.position.x, transform.position.y - unitPositionOffsetY), Vector3.one * .3f);
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawCube((Vector3)path[i], Vector3.one * .3f);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(new Vector2(transform.position.x, transform.position.y - unitPositionOffsetY), path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i-1], path[i]);
                }
            }
        }
    }
}
