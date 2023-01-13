using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kulip
{
    [CreateAssetMenu(fileName = "ScalableTime", menuName = "Kulip/Time/ScalableTime")]
    public class ScalableTimeSO : TimeSO
    {
        public System.Action<float> ScaleChangedEvent;

        public float Scale { 
            get => _scale; 
            set
            {
                if(value < 0)
                {
                    throw new System.ArgumentOutOfRangeException(
                        "ScalableTime.Scale",
                        value,
                        "Scale must be nonnegative"
                    );
                }
                _scale = value;
                ScaleChanged();
                ScaleChangedEvent?.Invoke(_scale);
            }
        }

        // Calculated by storing the base time and scaled time after every
        // scale change, and scaling the delta between the last scale base time
        // and current base time, adding the last saved scaled time
        public override float Time
            => (base.Time - _lastScaleChangedTime) * _scale
            + _lastScaleChangedScaledTime;

        public override float FixedTime
            => (base.Time - _lastScaleChangedFixedTime) * _scale
            + _lastScaleChangedScaledFixedTime;

        public override float DeltaTime => base.DeltaTime * _scale;
        public override float FixedDeltaTime => base.FixedDeltaTime * _scale;



        float _scale = 1f;

        float _lastScaleChangedTime = 0f;
        float _lastScaleChangedScaledTime = 0f;

        float _lastScaleChangedFixedTime = 0f;
        float _lastScaleChangedScaledFixedTime = 0f;

        void ScaleChanged()
        {
            _lastScaleChangedScaledTime = Time;
            _lastScaleChangedScaledFixedTime = FixedTime;

            _lastScaleChangedTime = base.Time;
            _lastScaleChangedFixedTime = base.FixedTime;
        }
    }
}
