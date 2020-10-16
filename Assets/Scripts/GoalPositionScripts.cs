using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;


public class GoalPositionScripts : MonoBehaviour
{
    public List<GameObject> target = new List<GameObject>();

    private void Start()
    {
        for (int i = 1; i < 7; i++)
        {
            target.Add(GameObject.Find("Tricky0" + i.ToString()));
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        
    }

    //private void Update()
    //{
    //    if (target != null)
    //    {
    //        if (target.gameObject.transform.position.y < 4)
    //        {
    //            target.transform.position += new Vector3(0, 1, 0) * Time.deltaTime;
    //        }
    //    }
    //}

    //IEnumerator TrickyTowerUp()
    //{

    //}
}
