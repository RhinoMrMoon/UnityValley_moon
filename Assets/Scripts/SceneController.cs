using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void RoadStage01()
    {
        SoundManager.instance.stop("IntroBGM");
        SoundManager.instance.play("StageSelect", 0.5f);
        SoundManager.instance.play("05 Water Palace");

        SceneManager.LoadScene("Stage01");
    }

    public void RoadStage02()
    {
        SoundManager.instance.stop("IntroBGM");
        SoundManager.instance.play("StageSelect", 0.5f);
        SoundManager.instance.play("16 Ida's Theme");

        SceneManager.LoadScene("Stage02");
    }

    public void ReloadScene()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Stage01":
                SoundManager.instance.stop("05 Water Palace");
                SoundManager.instance.play("05 Water Palace");
                SceneManager.LoadScene("Stage01");
                break;

            case "Stage02":
                SoundManager.instance.stop("16 Ida's Theme");
                SoundManager.instance.play("16 Ida's Theme");
                SceneManager.LoadScene("Stage02");
                break;
        }
    }

    public void RoadIntro()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Stage01": 
            {SoundManager.instance.stop("05 Water Palace");}
            break;

            case "Stage02":
            { SoundManager.instance.stop("16 Ida's Theme"); }
            break;
        }
        
        SceneManager.LoadScene("Intro");
    }
}
