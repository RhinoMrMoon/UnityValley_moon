using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Video;


public class VedioController : MonoBehaviour
{
    private VideoPlayer m_VideoPlayer;
    public GameObject plane;
    void Awake()
    {
        m_VideoPlayer = GetComponent<VideoPlayer>();
        m_VideoPlayer.loopPointReached += OnMovieFinished;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            plane.SetActive(false);
            //Destroy(plane);
        }

        if (!plane.activeSelf)
        {
            SoundManager.instance.play("IntroBGM");
        }
    }

    void OnMovieFinished(VideoPlayer player)
    {
        player.Stop();
        plane.SetActive(false);
    }
}
