using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void RoadStage01()
    {
        SoundManager1.instance.stop("IntroBGM");
        SoundManager1.instance.play("StageSelect", 0.5f);
        SoundManager1.instance.play("02 The Garden");

        SceneManager.LoadScene("Stage01");
    }

    public void RoadStage02()
    {
        SoundManager1.instance.stop("IntroBGM");
        SoundManager1.instance.play("StageSelect", 0.5f);
        SoundManager1.instance.play("16 Ida's Theme");

        SceneManager.LoadScene("Stage02");
    }

    public void ReloadScene()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Stage01":
                SoundManager1.instance.stop("02 The Garden");
                SoundManager1.instance.play("02 The Garden");
                SceneManager.LoadScene("Stage01");
                break;

            case "Stage02":
                SoundManager1.instance.stop("16 Ida's Theme");
                SoundManager1.instance.play("16 Ida's Theme");
                SceneManager.LoadScene("Stage02");
                break;
        }
    }

    public void RoadIntro()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Stage01": 
            {SoundManager1.instance.stop("02 The Garden");}
            break;

            case "Stage02":
            { SoundManager1.instance.stop("16 Ida's Theme"); }
            break;
        }
        
        SceneManager.LoadScene("Intro");
    }
}
