using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAction : FSMAction
{
    [Header("Config")]
    [SerializeField] private float speed;

    private Waypoint waypoint;

    private int index;  // 记录移动到哪个点

    private void Awake()
    {
        waypoint = GetComponent<Waypoint>();
    }

    public override void Act()
    {
        FollowPath();
    }

    private void FollowPath()
    {
        transform.position = Vector3.MoveTowards(transform.position, GetCurrentPosition(), speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, GetCurrentPosition()) <= 0.1f)
        {
            ChangeIndex();
        }
    }

    private void ChangeIndex()
    {
        index++;
        if (index == waypoint.Points.Length)
            index = 0;
    }

    private Vector3 GetCurrentPosition()
    {
        return waypoint.GetPosition(index);
    }
}
