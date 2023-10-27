using Kulip;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EasingSampleUI : MonoBehaviour
{
    [SerializeField] Text timeText;
    [SerializeField] Dropdown ball1EaseInOutDropdown;
    [SerializeField] Dropdown ball1EaseTypeDropdown;
    [SerializeField] Dropdown ball2EaseInOutDropdown;
    [SerializeField] Dropdown ball2EaseTypeDropdown;
    [SerializeField] Slider animDurationSlider;
    [SerializeField] Button pausePlayButton;
    [SerializeField] Text pausePlayButtonText;
    [SerializeField] Toggle repeatToggle;
    [SerializeField] Transform ball1;
    [SerializeField] Transform ball2;
    [SerializeField] float ballMinX;
    [SerializeField] float ballMaxX;

    float animDuration;
    float progress = 0.0f;

    bool playing = false;
    bool animEnded = false;

    System.Func<float, float> ball1EaseFunc = null;
    System.Func<float, float> ball2EaseFunc = null;

    void Start()
    {
        ball1EaseInOutDropdown.onValueChanged.AddListener(UpdateBallEaseFunctions);
        ball1EaseTypeDropdown.onValueChanged.AddListener(UpdateBallEaseFunctions);
        ball2EaseInOutDropdown.onValueChanged.AddListener(UpdateBallEaseFunctions);
        ball2EaseTypeDropdown.onValueChanged.AddListener(UpdateBallEaseFunctions);

        animDuration = animDurationSlider.value;
        animDurationSlider.onValueChanged.AddListener(OnAnimDurationChange);

        pausePlayButton.onClick.AddListener(OnPausePlay);

        UpdateProgressText();
        UpdateBallEaseFunctions(0);
    }

    void OnAnimDurationChange(float value)
    {
        animDuration = value;
    }

    void OnPausePlay()
    {
        if (playing)
        {
            Pause();
        }
        else
        {
            Play();
        }
    }

    void Pause()
    {
        playing = false;
        pausePlayButtonText.text = "Play";
    }

    void Play()
    {
        playing = true;
        if (animEnded)
        {
            animEnded = false;
            progress = 0;
        }
        pausePlayButtonText.text = "Pause";
    }

    void Update()
    {
        if (playing)
        {
            progress += Time.deltaTime / animDuration;
            if (progress >= 1 && repeatToggle.isOn)
            {
                progress = 0;
            }
            else if(progress >= 1)
            {
                progress = 1;
                animEnded = true;
                Pause();
            }
            UpdateBalls();
            UpdateProgressText();
        }
    }

    void UpdateProgressText()
    {
        timeText.text = $"Progress: {progress}";
    }

    void UpdateBalls()
    {
        
        ball1.position = new(Mathf.LerpUnclamped(ballMinX, ballMaxX, ball1EaseFunc(progress)), ball1.position.y, ball1.position.z);
        ball2.position = new(Mathf.LerpUnclamped(ballMinX, ballMaxX, ball2EaseFunc(progress)), ball2.position.y, ball2.position.z);
    }

    void UpdateBallEaseFunctions(int _)
    {
        Debug.Log(ball1EaseTypeDropdown.value);
        string ball1Type = ball1EaseTypeDropdown.options[ball1EaseTypeDropdown.value].text;
        string ball1InOut = ball1EaseInOutDropdown.options[ball1EaseInOutDropdown.value].text;
        string ball2Type = ball2EaseTypeDropdown.options[ball2EaseTypeDropdown.value].text;
        string ball2InOut = ball2EaseInOutDropdown.options[ball2EaseInOutDropdown.value].text;

        ball1EaseFunc = GetEaseFuncFromName(ball1Type, ball1InOut);
        ball2EaseFunc = GetEaseFuncFromName(ball2Type, ball2InOut);
    }

    System.Func<float, float> GetEaseFuncFromName(string type, string inOut)
    {
        type = type.ToLower();
        inOut = inOut.ToLower();
        switch (type.ToLower())
        {
            case "linear": return Ease.Linear;
            case "sine":
                return inOut == "in"
                    ? Ease.InSine
                    : inOut == "out"
                    ? Ease.OutSine
                    : inOut == "inout"
                    ? Ease.InOutSine
                    : null;
            case "quad":
                return inOut == "in"
                    ? Ease.InQuad
                    : inOut == "out"
                    ? Ease.OutQuad
                    : inOut == "inout"
                    ? Ease.InOutQuad
                    : null;
            case "cubic":
                return inOut == "in"
                    ? Ease.InCubic
                    : inOut == "out"
                    ? Ease.OutCubic
                    : inOut == "inout"
                    ? Ease.InOutCubic
                    : null;
            case "quart":
                return inOut == "in"
                    ? Ease.InQuart
                    : inOut == "out"
                    ? Ease.OutQuart
                    : inOut == "inout"
                    ? Ease.InOutQuart
                    : null;
            case "quint":
                return inOut == "in"
                    ? Ease.InQuint
                    : inOut == "out"
                    ? Ease.OutQuint
                    : inOut == "inout"
                    ? Ease.InOutQuint
                    : null;
            case "expo":
                return inOut == "in"
                    ? Ease.InExpo
                    : inOut == "out"
                    ? Ease.OutExpo
                    : inOut == "inout"
                    ? Ease.InOutExpo
                    : null;
            case "circ":
                return inOut == "in"
                    ? Ease.InCirc
                    : inOut == "out"
                    ? Ease.OutCirc
                    : inOut == "inout"
                    ? Ease.InOutCirc
                    : null;
            case "back":
                return inOut == "in"
                    ? Ease.InBack
                    : inOut == "out"
                    ? Ease.OutBack
                    : inOut == "inout"
                    ? Ease.InOutBack
                    : null;
            case "elastic":
                return inOut == "in"
                    ? Ease.InElastic
                    : inOut == "out"
                    ? Ease.OutElastic
                    : inOut == "inout"
                    ? Ease.InOutElastic
                    : null;
            case "bounce":
                return inOut == "in"
                    ? Ease.InBounce
                    : inOut == "out"
                    ? Ease.OutBounce
                    : inOut == "inout"
                    ? Ease.InOutBounce
                    : null;
        }
        return null;
    }
}
