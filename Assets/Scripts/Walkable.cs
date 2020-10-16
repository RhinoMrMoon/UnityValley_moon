using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walkable : MonoBehaviour
{
    // 해당 블록에서 이동할 수 있는 좌표
    public List<WalkPath> PossiblePaths = new List<WalkPath>();

    [Space]
    // 지나온 길
    public Transform previousBlock;

    [Header("Booleans")]
    // 계단 여부
    public bool isStair = false;
    // 움직이는 오브젝트 여부
    public bool movingGround = false;
    public bool isButton;
    // 플레이어가 바라보는 방향 변경 여부
    public bool dontRotate;
    public bool tricky;
    public bool isTop = false;

    [Space]
    [Header("offsets")]
    public float walkPointOffset = .5f;
    public float stairOffset = .4f;

    //걷는 좌표 구하는 함수
    public Vector3 GetWalkPoint()
    {
        //계단 여부
        float stair = isStair ? stairOffset : 0;
        // 좌표 반환
        return transform.position + transform.up * walkPointOffset - transform.up * stair;
    }

    private void OnDrawGizmos()
    {
        //큐브 위에 타겟 그리기
        Gizmos.color = Color.gray;
        //float stair = isStair ? .4f : 0;
        Gizmos.DrawCube(GetWalkPoint(), new Vector3(0.1f, 0.1f, 0.1f));
        //Gizmos.DrawSphere(GetWalkPoint(), .1f);

        //if (PossiblePaths == null) return;

        //foreach (WalkPath p in PossiblePaths)
        //{
        //    if (p.target == null) return;
        //    Gizmos.color = p.active ? Color.black : Color.clear;
        //    Gizmos.DrawLine(GetWalkPoint(), p.target.GetComponent<Walkable>().GetWalkPoint());
        //}

        //for (int i = 0; i < PossiblePaths.Count; i++)
        //{
        //    if (tricky)
        //    {
        //        PossiblePaths[i].active = false;
        //    }
        //}

        for (int i = 0; i < PossiblePaths.Count; i++)
        {
            //if (tricky)
            //{
            //    PossiblePaths[i].active = false;
            //}
            //if (!tricky)
            //{
            //    PossiblePaths[i].active = true;
            //}

            if (PossiblePaths[i].active)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawLine(GetWalkPoint(), PossiblePaths[i].target.GetComponent<Walkable>().GetWalkPoint());
            }
        }
    }
}

[System.Serializable]
public class WalkPath
{
    public Transform target;
    public bool active = true;
}