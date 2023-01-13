using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kulip
{
    public class StaticSO<T> : ScriptableObject
    {
        [SerializeField] T _defaultValue;

        public System.Action<T> ValueChangedEvent;

        T _value;
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                ValueChangedEvent?.Invoke(_value);
            }
        }

        void OnEnable()
        {
            _value = _defaultValue;
        }
    }
}
