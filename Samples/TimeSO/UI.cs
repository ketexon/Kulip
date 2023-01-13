using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] Kulip.ScalableTimeSO time1;
    [SerializeField] Kulip.ScalableTimeSO time2;

    [SerializeField] Slider scaleSlider1;
    [SerializeField] Slider scaleSlider2;
    [SerializeField] Button pauseButton1;
    [SerializeField] Button pauseButton2;
    [SerializeField] Text pauseButtonText1;
    [SerializeField] Text pauseButtonText2;

    [SerializeField] Text timeText;

    void Reset()
    {
        if(pauseButton1 != null)
        {
            pauseButtonText1 = pauseButton1.GetComponentInChildren<Text>();
        }
        if(pauseButton2 != null)
        {
            pauseButtonText2 = pauseButton2.GetComponentInChildren<Text>();
        }
    }

    void Start()
    {
        pauseButtonText1.text = "Pause";
        pauseButtonText2.text = "Pause";

        scaleSlider1.onValueChanged.AddListener(
            v => time1.Scale = v
        );
        scaleSlider2.onValueChanged.AddListener(
            v => time2.Scale = v
        );
        pauseButton1.onClick.AddListener(
            () =>
            {
                if (time1.Paused)
                {
                    pauseButtonText1.text = "Pause";
                    time1.Resume();
                }
                else
                {
                    pauseButtonText1.text = "Resume";
                    time1.Pause();
                }
            }
        );

        pauseButton2.onClick.AddListener(
            () =>
            {
                if (time2.Paused)
                {
                    pauseButtonText2.text = "Pause";
                    time2.Resume();
                }
                else
                {
                    pauseButtonText2.text = "Resume";
                    time2.Pause();
                }
            }
        );
    }

    void Update()
    {
        timeText.text = string.Format(
            "Time 1: {0}\nTime 2: {1}\nUnity Time: {2}", 
            time1.Time,
            time2.Time,
            Time.time
        );
    }
}
