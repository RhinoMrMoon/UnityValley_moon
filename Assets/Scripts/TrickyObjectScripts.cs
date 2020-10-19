using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TrickyObjectScripts : MonoBehaviour
{
    public enum tagDirection
    {
        Up, Down, North, South, East, West
    }

    //public List<GameObject> target = new List<GameObject>();
    // 오브젝트 이름
    //public string TrickyTowerName;
    // 나타날 방향
    public tagDirection dir;

    public float offset;

    public Transform target;
    public Transform target02;

    //public List<Transform> targets;

    //public List<Walkable> block = new List<Walkable>();
    public List<soloPaths> WalkBlock = new List<soloPaths>();

    public float speed;

    bool trig = false;
    bool pathActive;
    Vector3 targetPos;

    // 순차적으로 올라오기 위해 사용할 변수
    int currentIndex = 0;
    int totalIndex;
    float timing = 0;

    RotObject rotObject;
    Walkable TrickyTowerObject;

    void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            target.GetChild(i).GetChild(3).gameObject.GetComponent<Walkable>().enabled = false;
        }

        target.gameObject.SetActive(false);

        target02.GetChild(0).gameObject.GetComponent<RotObject02>().enabled = false;
        target02.gameObject.SetActive(false);

        //pathActive = false;
        //rotObject = GameObject.Find("Lever").GetComponent<RotObject>();
        //TrickyTowerObject = GameObject.Find("TrickyObject").GetComponent<Walkable>();


        //switch (dir)
        //{
        //    case tagDirection.Up:
        //        target.position = new Vector3(target.position.x, target.position.y - offset, target.position.z);
        //    break;

        //    case tagDirection.Down:
        //        target.position = new Vector3(target.position.x, target.position.y + offset, target.position.z);
        //    break;

        //    case tagDirection.North:
        //        target.position = new Vector3(target.position.x - offset, target.position.y, target.position.z);
        //        break;

        //    case tagDirection.South:
        //        target.position = new Vector3(target.position.x + offset, target.position.y, target.position.z);
        //        break;

        //    case tagDirection.East:
        //        target.position = new Vector3(target.position.x, target.position.y, target.position.z + offset);
        //        break;

        //    case tagDirection.West:
        //        target.position = new Vector3(target.position.x, target.position.y, target.position.z - offset);
        //        break;
        //}

        //for (int i = 0; i < block.Count; i++)
        //{
        //    for (int j = 0; j < block[i].PossiblePaths.Count; j++)
        //    {
        //        block[i].PossiblePaths[j].active = trig;
        //    }
        //}

        //if (!trig)
        //{
        //    transform.GetComponent<WalkPath>().active = false;
        //}
    }

    void Update()
    {
        //if (!trig)
        //{
        //    target.GetComponentInChildren<WalkPath>().active = false;
        //target.GetComponentInChildren<WalkPath>().active = true;

        //}

        //if (rotObject.currentRotate && pathActive)
        //{
        //    for (int i = 0; i < rotObject.pathCubes.Count; i++)
        //    {
        //        for (int j = 0; j < rotObject.pathCubes[i].path.Count; j++)
        //        {
        //            WalkBlock[i].block.PossiblePaths[j].active = pathActive;
        //        }
        //    }
        //}

        //for (int i = 0; i < rotObject.pathCubes.Count; i++)
        //{
        //    for (int j = 0; j < rotObject.pathCubes[i].path.Count; j++)
        //    {
        //        //GameObject LeverClone = GameObject.Find("Lever").GetComponent<GameObject>();

        //        if (rotObject.currentRotate)
        //        {
        //            //block[i].PossiblePaths[j].active = pathActive;
        //            if (WalkBlock.Count == i)
        //            {
        //                WalkBlock. = pathActive;
        //            }
        //        }
        //    }
        //}

        if (trig)
        {
            timing += Time.deltaTime * speed;

            // 간격
            if (timing >= speed / 3.0f)
            {
                StartCoroutine(MoveCo(currentIndex++));

                if (currentIndex >= target.childCount) trig = false;

                timing = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        target.gameObject.SetActive(true);
        trig = true;

        SoundManager.instance.play("trigger_lock_Sound");

        for (int i = 0; i < 6; i++)
        {
            target.GetChild(i).GetChild(3).gameObject.GetComponent<Walkable>().enabled = true;
        }

        //pathActive = true;
        //target.GetComponentInChildren<Walkable>().tricky = false;


        //Vector3 effectPos = targetPos;
        //effectPos.y = targetPos.y - offset;
        //GameObject.Find("AuraTarget01").transform.position = effectPos;

        //Debug.Log("dasda");

        StartCoroutine("MoveCo", currentIndex++);
        //StartCoroutine(MoveCo(currentIndex++));

        //for (int i = 0; i < block.PossiblePaths.Count; i++)
        //{
        //    block.PossiblePaths[i].active = trig;
        //}

        targetPos = transform.position;

    }

    IEnumerator MoveCo(int index)
    {

        Vector3 startPos = target.GetChild(index).localPosition;
        Vector3 finalPos = target.GetChild(index).localPosition;

        switch (dir)
        {
            case tagDirection.Up:
                finalPos.y += offset;
                break;
            case tagDirection.Down:
                finalPos.y -= offset;
                break;
            case tagDirection.North:
                finalPos.x += offset;
                break;
            case tagDirection.South:
                finalPos.x -= offset;
                break;
            case tagDirection.East:
                finalPos.z -= offset;
                break;
            case tagDirection.West:
                finalPos.z += offset;
                break;
        }

        Vector3 targetLastPos = targetPos;
        targetLastPos.y -= offset;
        

        //finalPos.y += offset;

        float timing = 0;

        while (timing <= 1.0f)
        {
            target.GetChild(index).localPosition = Vector3.Lerp(startPos, finalPos, timing);
            transform.localPosition = Vector3.Lerp(targetPos, targetLastPos, timing);
            timing += Time.deltaTime * speed;

            yield return null;
        }

        target.GetChild(index).localPosition = finalPos;
        transform.position = targetLastPos;

        //yield return null;
    }

    public bool possiblePathWalk
    {
        get { return pathActive; }
    }
}

//public class pathCondition
//{
//    public List<soloPaths> pathPath;
//}

public class soloPaths
{
    public Transform go;
    public Walkable block;
    public int index;
}