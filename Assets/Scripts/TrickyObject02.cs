using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickyObject02 : MonoBehaviour
{
    public enum tagDirection
    {
        Up, Down, North, South, East, West
    }

    public tagDirection dir;
    public float offset;

    public Transform target;

    public Transform target02;

    public float speed;
    bool trig = false;
    Vector3 targetPos;



    // 순차적으로 올라오기 위해 사용할 변수
    int currentIndex = 0;
    float timing = 0;

    // Start is called before the first frame update
    void Start()
    {
        //target02.GetChild(0).gameObject.GetComponent<RotObject02>().enabled = false;
        //target02.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (trig)
        {
            timing += Time.deltaTime * speed;

            // 간격
            //if (timing >= speed / 3.0f)
            //{
            //    //StartCoroutine(MoveCo(currentIndex++));
            //    StartCoroutine(MoveCo02());

            //    if (currentIndex >= target.childCount) trig = false;

            //    timing = 0;
            //}
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        trig = true;

        target02.position = target.position;
        target02.rotation = target.rotation;
        target02.gameObject.SetActive(true);

        target.gameObject.SetActive(false);

        if (target02.gameObject.activeInHierarchy == true)
        {
            target02.GetChild(0).gameObject.GetComponent<RotObject02>().enabled = true;
        }

        StartCoroutine(MoveCo02());

        //StartCoroutine("MoveCo", currentIndex++);
        targetPos = transform.position;

       
    }

    IEnumerator MoveCo02()
    {
        Vector3 startPos = target.position;
        Vector3 lastPos = target02.position;

        switch (dir)
        {
            case tagDirection.Up:
                lastPos.y += offset;
                break;
            case tagDirection.Down:
                lastPos.y -= offset;
                break;
            case tagDirection.North:
                lastPos.x += offset;
                break;
            case tagDirection.South:
                lastPos.x -= offset;
                break;
            case tagDirection.East:
                lastPos.z -= offset;
                break;
            case tagDirection.West:
                lastPos.z += offset;
                break;
        }

        Vector3 targetLastPos = targetPos;
        targetLastPos.y -= offset;

        float timing = 0;

        while (timing <= 1.0f)
        {
            target02.position = Vector3.Lerp(startPos, lastPos, timing);
            transform.position = Vector3.Lerp(targetPos, targetLastPos, timing);
            timing += Time.deltaTime * speed;

            //yield return null;
        }

        target02.position = lastPos;
        transform.position = targetLastPos;

        yield return null;
    }

    //IEnumerator MoveCo(int index)
    //{

    //    Vector3 startPos = target.GetChild(index).position;
    //    Vector3 finalPos = target.GetChild(index).position;

    //    switch (dir)
    //    {
    //        case tagDirection.Up:
    //            finalPos.y += offset;
    //            break;
    //        case tagDirection.Down:
    //            finalPos.y -= offset;
    //            break;
    //        case tagDirection.North:
    //            finalPos.x += offset;
    //            break;
    //        case tagDirection.South:
    //            finalPos.x -= offset;
    //            break;
    //        case tagDirection.East:
    //            finalPos.z -= offset;
    //            break;
    //        case tagDirection.West:
    //            finalPos.z += offset;
    //            break;
    //    }

    //    Vector3 targetLastPos = targetPos;
    //    targetLastPos.y -= offset;

    //    float timing = 0;

    //    while (timing <= 1.0f)
    //    {
    //        target.GetChild(index).position = Vector3.Lerp(startPos, finalPos, timing);
    //        transform.position = Vector3.Lerp(targetPos, targetLastPos, timing);
    //        timing += Time.deltaTime * speed;

    //        yield return null;
    //    }

    //    target.GetChild(index).position = finalPos;
    //    transform.position = targetLastPos;

    //    yield return null;
    //}
}
