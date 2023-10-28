using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSOExampleMovingCircle : MonoBehaviour
{
    [SerializeField] Kulip.TimeSO time;
    [SerializeField] float movementRadius;

    private void Update()
    {
        transform.localPosition = new Vector3(
            Mathf.Cos(time.Time),
            Mathf.Sin(time.Time),
            0
        ) * movementRadius;
    }
}
