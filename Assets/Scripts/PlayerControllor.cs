﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerControllor : MonoBehaviour
{
    [Space]
    [Header("Cube Information")]
    // 현재 위치한 큐브
    public Transform currentCube;
    // 마우스 클릭한 큐브
    public Transform clickedCube;

    [Space]
    [Header("Animation")]
    // 애니메이션
    public Animator anim;
    public ParticleSystem clickEffect;

    [Space]
    [Header("PathFinding")]
    // 플레이어가 실제 이동할 경로
    public List<Transform> finalPath = new List<Transform>();

    // 이동 시 사용할 변수
    Walkable pastCube;
    Walkable nextCube;
    float timing = 0;
    float triggerTiming = 0;
    bool trig = false;

    [Space]
    public float moveSpeed;

    public Transform LeverTarget;

    void Start()
    {
        // 플레이어가 밟고 있는 큐브 설정
        RayCastDown();

        // 플레이어 레이어 설정
        //LayerCheck(currentCube);

        if (currentCube.GetComponent<Walkable>().isTop)
        {
            SetLayerObject(transform, "Top");
        }
        else
        {
            SetLayerObject(transform, "Default");
        }
    }

    void Update()
    {
        // 플레이어가 밟고 있는 큐브 설정
        RayCastDown();

        if (currentCube.gameObject.name == "MiddleGoalCube")
        {
            triggerTiming += Time.deltaTime;
        }

        if (triggerTiming > 0 && triggerTiming <= 3.0f)
        {
            trig = true;
        }
        else
        {
            trig = false;
        }

        

        if (currentCube.GetComponent<Walkable>().isTop)
        {
            SetLayerObject(transform, "Top");
        }
        else
        {
            SetLayerObject(transform, "Default");
        }


        // 현재 밟고 있는 큐브가 움직이는 경우
        if (currentCube.GetComponent<Walkable>().movingGround)
        {
            // 플레이어를 그 자식으로 넣는다
            transform.parent = currentCube.parent;
        }
        else
        {
            transform.parent = null;
        }

        if (!trig)
        {
            // 마우스 클릭과 게임매니저 플래그 체크
            if (Input.GetMouseButtonDown(0) && GameManager.instance.Ready)
            {
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit mouseHit;

                //anim.SetBool("Walking", true);

                // 레이 발사
                if (Physics.Raycast(mouseRay, out mouseHit))
                {
                    // 클릭한 곳이 Path인 경우
                    // Walkable 스크립트가 null값이 아니라면
                    if (mouseHit.transform.GetComponent<Walkable>() != null)
                    {
                        // 클릭음 재생
                        SoundManager.instance.play("Navi", 0.5f);

                        // 클릭한 큐브 위치 설정
                        clickedCube = mouseHit.transform;


                        // 이펙트 위치 조정
                        clickEffect.transform.position = clickedCube.GetComponent<Walkable>().GetWalkPoint();

                        // 이펙트 레이어 조정
                        if (clickedCube.childCount > 0)
                        {
                            SetLayerObject(clickEffect.transform, "Top");
                        }
                        else
                        {
                            SetLayerObject(clickEffect.transform, "Default");
                        }

                        // 이펙트 재생
                        clickEffect.Play();

                        // 경로 초기화
                        finalPath.Clear();

                        // 길찾기 시작
                        FindPath();
                    }
                }
            }

            FollowPath();
        }
    }

    // 길찾기
    private void FindPath()
    {
        // 다음 이동할 큐브
        List<Transform> nextCubes = new List<Transform>();
        // 이전 큐브
        List<Transform> pastCubes = new List<Transform>();

        // 현재 큐브의 연결된 큐브 갯수만큼 루프
        for (int i = 0; i < currentCube.GetComponent<Walkable>().PossiblePaths.Count; i++)
        {
            WalkPath walkPath = currentCube.GetComponent<Walkable>().PossiblePaths[i];

            if (walkPath.active)
            {
                nextCubes.Add(walkPath.target);

                walkPath.target.GetComponent<Walkable>().previousBlock = currentCube;
            }
        }

        pastCubes.Add(currentCube);

        ExploreCube(nextCubes, pastCubes);
        BuildPath();
    }

    private void ExploreCube(List<Transform> nextCubes, List<Transform> visitedCubes)
    {
        Transform current = nextCubes.First();
        nextCubes.Remove(current);

        // 클릭한 큐브와 현재 큐브가 같으면
        // 목표 좌표에 도착한 것
        if (current == clickedCube)
        {
            return;
        }

        // 현재 큐브의 이동 가능한 큐브만큼 반복
        for (int i = 0; i < current.GetComponent<Walkable>().PossiblePaths.Count; i++)
        {
            WalkPath walkPath = current.GetComponent<Walkable>().PossiblePaths[i];

            // 이미 지나온 길이 아니고 길이 연결되어 있다면
            if (!visitedCubes.Contains(walkPath.target) && walkPath.active)
            {
                // 다음 검색 큐브로
                nextCubes.Add(walkPath.target);

                // 이동할 경로에 추가
                walkPath.target.GetComponent<Walkable>().previousBlock = current;
            }

        }

        // 방문한 큐브 리스트에 현재 큐브 추가
        visitedCubes.Add(current);

        // 리스트가 하나라도 있다면
        if (nextCubes.Count > 0)
        {
            ExploreCube(nextCubes, visitedCubes);
        }
    }

    // 경로 생성
    private void BuildPath()
    {
        Transform cube = clickedCube;

        // 클릭한 큐브가 현재큐브와 같지 않을 때까지
        while (cube != currentCube)
        {
            // 실제 이동할 경로에 삽입
            finalPath.Add(cube);

            // 클릭한 큐브의 이전큐브가 None일 때
            if (cube.GetComponent<Walkable>().previousBlock != null)
            {
                cube = cube.GetComponent<Walkable>().previousBlock;
            }
            else
            {
                break;
            }
        }

        // 경로가 하나 이상이면
        if (finalPath.Count > 0)
        {
            bool walk = false;

            for (int i = 0; i < currentCube.GetComponent<Walkable>().PossiblePaths.Count; i++)
            {
                WalkPath walkCube = currentCube.GetComponent<Walkable>().PossiblePaths[i];

                if (walkCube.target.Equals(finalPath[finalPath.Count - 1]) && walkCube.active)
                {
                    walk = true;
                    break;
                }
            }

            if (!walk)
            {
                finalPath.Clear();
            }
            else
            {
                pastCube = currentCube.GetComponent<Walkable>();
                nextCube = finalPath[finalPath.Count - 1].GetComponent<Walkable>();

                if (!nextCube.isStair)
                {
                    transform.LookAt(nextCube.GetWalkPoint());
                }

                if (nextCube.isStair)
                {
                    Vector3 nextPos;
                    nextPos.x = nextCube.GetWalkPoint().x;
                    nextPos.y = pastCube.GetWalkPoint().y;
                    nextPos.z = nextCube.GetWalkPoint().z;

                    transform.LookAt(nextPos);
                }

                timing = 0;

                anim.SetBool("Walking", true);
            }
        }
    }

    void FollowPath()
    {
        if (finalPath.Count == 0)
        {
            return;
        }

        // 타일을 체크하여 플레이어 레이어 설정
        //LayerCheck(nextCube.transform);

        // 보간 적용
        transform.position = Vector3.Lerp(pastCube.GetWalkPoint(), nextCube.GetWalkPoint(), timing);

        // 다음 경로 설정
        if (timing >= 1.0f)
        {
            timing = 0;

            pastCube = finalPath.Last().GetComponent<Walkable>();

            finalPath.Last().GetComponent<Walkable>().previousBlock = null;
            finalPath.RemoveAt(finalPath.Count - 1);

            if (finalPath.Count > 0)
            {
                nextCube = finalPath.Last().GetComponent<Walkable>();

                if (!currentCube.GetComponent<Walkable>().dontRotate)
                {
                    if (!nextCube.isStair)
                    {
                        transform.LookAt(nextCube.GetWalkPoint());
                    }

                    if (nextCube.isStair)
                    {
                        Vector3 nextPos;
                        nextPos.x = nextCube.GetWalkPoint().x;
                        nextPos.y = pastCube.GetWalkPoint().y;
                        nextPos.z = nextCube.GetWalkPoint().z;

                        transform.LookAt(nextPos);
                    }
                }

                return;
            }
            else
            {
                //LayerCheck(nextCube.transform);

                anim.SetBool("Walking", false);

                if (currentCube.Equals(GameManager.instance.clearCube))
                {
                    //anim.SetBool("Clear", true);

                    GameManager.instance.Clear = true;
                }
            }
        }

        timing += Time.deltaTime * moveSpeed;
    }

    // 현재 플레이어가 밟고 있는 큐브 찾는 함수
    public void RayCastDown()
    {
        // 플레이어 센터 포지션 생성
        Vector3 rayPos = transform.position;
        rayPos.y += transform.localScale.y * 0.5f;

        // 레이 생성, 방향은 아래
        Ray playerRay = new Ray(rayPos, -transform.up);
        RaycastHit playerHit;

        // 레이 발사
        if (Physics.Raycast(playerRay, out playerHit))
        {
            // 발판을 밟고 있다면
            if (playerHit.transform.GetComponent<Walkable>() != null)
            {
                currentCube = playerHit.transform;
            }
        }
    }

    // 발판을 검사하여 플레이어의 레이어 설정
    void LayerCheck(Transform cube)
    {
        bool isTop = false;

        if (cube.childCount > 0)
        {
            isTop = true;
        }

        if (!isTop)
        {
            for (int i = 0; i < cube.GetComponent<Walkable>().PossiblePaths.Count; i++)
            {
                if (cube.GetComponent<Walkable>().PossiblePaths[i].target.childCount > 0)
                {
                    isTop = true;
                    break;
                }
            }
        }

        if (isTop)
        {
            SetLayerObject(transform, "Top");
        }
        else
        {
            SetLayerObject(transform, "Default");
        }
    }

    // 자식 오브젝트까지 모두 레이어 설정
    void SetLayerObject(Transform tr, string layerName)
    {
        tr.gameObject.layer = LayerMask.NameToLayer(layerName);

        for (int i = 0; i < tr.childCount; i++)
        {
            // 자식의 자식들까지 설정
            SetLayerObject(tr.GetChild(i), layerName);
        }
    }

    Transform getCurrentCube()
    {
        return currentCube;
    }
}