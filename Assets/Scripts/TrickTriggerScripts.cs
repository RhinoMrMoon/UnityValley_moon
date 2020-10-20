using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickTriggerScripts : MonoBehaviour
{
    public enum tagDirection
    {
        Up, Down, North, South, East, West
    }

    public tagDirection dir;
    public float offset;

    public Transform target;

    //public Transform target02;

    public float speed;
    bool trig = false;
    Vector3 targetPos;



    // 순차적으로 올라오기 위해 사용할 변수
    int currentIndex = 0;
    float timing = 0;

    // Start is called before the first frame update
    void Start()
    {
        //target02.gameObject.SetActive(false);

        //for (int i = 0; i < target02.transform.childCount; i++)
        //{
        //    target02.GetChild(i).gameObject.GetComponent<Walkable>().enabled = false;
        //}

        //for (int i = 0; i < target02.childCount; i++)
        //{
        //    target02.GetChild(i).GetComponentInChildren<Walkable>().enabled = false;
        //    target02.GetChild(i).gameObject.SetActive(false);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //if (!trig)
        //{
        //    for (int i = 0; i < target02.transform.childCount; i++)
        //    {
        //        target02.GetChild(i).gameObject.GetComponent<Walkable>().enabled = false;
        //    }
        //}

        if (trig)
        {
            timing += Time.deltaTime * speed;

            if (true)
            {

            }
            //gameObject.transform.Find("Player").GetComponentInChildren<PlayerControllor>().enabled = false;
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

        SoundManager.instance.play("trigger_lock_Sound");

        trig = true;

        //target.gameObject.SetActive(false);

        StartCoroutine(MoveCo02());

        //StartCoroutine("MoveCo", currentIndex++);
        targetPos = transform.position;

    }

    IEnumerator MoveCo02()
    {
        Vector3 startPos = target.position;
        Vector3 lastPos = target.position;

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
            target.position = Vector3.Lerp(startPos, lastPos, timing);
            transform.position = Vector3.Lerp(targetPos, targetLastPos, timing);
            timing += Time.deltaTime * speed;

            yield return null;
        }

        //if (timing >= 1.0f)
        //{
        //    for (int i = 0; i < target02.childCount; i++)
        //    {
        //        target02.GetChild(i).gameObject.SetActive(true);
        //        //target02.GetChild(i).GetComponentInChildren<Walkable>().enabled = true;
        //    }
        //}

        //if (timing >= 1.0f)
        //{
        //    for (int i = 0; i < target02.transform.childCount; i++)
        //    {
        //        target02.GetChild(i).gameObject.GetComponent<Walkable>().enabled = true;
        //    }
        //}

        //if (timing >= 1.0f)
        //{
        //    target02.gameObject.SetActive(true);

        //    for (int i = 0; i < target02.transform.childCount; i++)
        //    {
        //        target02.GetChild(i).gameObject.GetComponent<Walkable>().enabled = false;
        //    }
        //}

        //target.position = lastPos;
        //transform.position = targetLastPos;

        //yield return null;
    }
}
