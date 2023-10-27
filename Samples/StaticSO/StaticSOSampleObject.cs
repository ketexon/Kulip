using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kulip.Samples
{
    internal class StaticSOSampleObject : MonoBehaviour
    {
        [SerializeField] Kulip.StaticSO<float> staticSO;

        void Awake()
        {
            staticSO.ValueChangedEvent += OnValueChanged;
            OnValueChanged(staticSO.Value);
        }

        void OnDestroy()
        {
            staticSO.ValueChangedEvent -= OnValueChanged;
        }

        void OnValueChanged(float value)
        {
            transform.position = new Vector3(
                value,
                transform.position.y,
                transform.position.z
            );
        }
    }
}