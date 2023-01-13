using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Kulip
{
    public class UnityEventSO : ScriptableObject
    {
        UnityEvent _event;

        public static UnityEventSO operator +(UnityEventSO a, UnityAction f)
        {
            a._event.AddListener(f);
            return a;
        }

        public static UnityEventSO operator -(UnityEventSO a, UnityAction f)
        {
            a._event.RemoveListener(f);
            return a;
        }

        public void Invoke()
        {
            _event.Invoke();
        }

        public void Clear()
        {
            _event.RemoveAllListeners();
        }
    }

    public class UnityEventSO<T> : ScriptableObject
    {
        UnityEvent<T> _event;

        public static UnityEventSO<T> operator +(UnityEventSO<T> a, UnityAction<T> f)
        {
            a._event.AddListener(f);
            return a;
        }

        public static UnityEventSO<T> operator -(UnityEventSO<T> a, UnityAction<T> f)
        {
            a._event.RemoveListener(f);
            return a;
        }

        public void Invoke(T t)
        {
            _event.Invoke(t);
        }

        public void Clear()
        {
            _event.RemoveAllListeners();
        }
    }
}
