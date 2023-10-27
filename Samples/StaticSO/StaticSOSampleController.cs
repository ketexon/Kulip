using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Kulip.Samples
{
    internal class StaticSOSampleController : MonoBehaviour
    {
        [SerializeField] Text text;
        [SerializeField] Slider slider;
        [SerializeField] Kulip.StaticSO<float> staticSO;

        void Reset()
        {
            text = GetComponentInChildren<Text>();
            slider = GetComponentInChildren<Slider>();
        }

        void Awake()
        {
            slider.onValueChanged.AddListener(OnSliderChanged);
            slider.value = staticSO.Value;
            UpdateText();
        }

        void OnDestroy()
        {
            slider.onValueChanged.RemoveAllListeners();
        }

        void OnSliderChanged(float value)
        {
            staticSO.Value = value;
            UpdateText();
        }

        void UpdateText()
        {
            text.text = $"Static Float: {staticSO.Value}";
        }
    }

}