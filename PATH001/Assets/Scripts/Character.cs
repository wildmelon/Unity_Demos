using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Character : MonoBehaviour
{
    public float velocity = 1.0f;
    public bool hasTarget;
    public Vector3Int cellPosition;

    private Vector3 mTargetPos;

    void Update()
    {
        if (hasTarget)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, mTargetPos, Time.deltaTime * velocity);
            if (CloseTo(transform.localPosition, mTargetPos))
            {
                hasTarget = false;
            }
        }

    }

    public void SetCellPosition(Tilemap map , Vector3Int cellPos)
    {
        cellPosition = cellPos;
        Vector3 destPos = map.GetCellCenterWorld(cellPos);
        this.transform.localPosition = destPos;
    }

    public void MoveTo(Vector3 targetPos)
    {
        mTargetPos = targetPos;
        hasTarget = !CloseTo(transform.localPosition, mTargetPos);
    }

    private bool CloseTo(Vector2 vector1, Vector2 vector2)
    {
        return ((vector1 - vector2).sqrMagnitude < 0.0001);
    }
}
