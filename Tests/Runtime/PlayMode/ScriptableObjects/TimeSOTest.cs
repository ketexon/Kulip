using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TimeSOTest
{
    const float waitTime = 2f;

    [UnityTest]
    public IEnumerator TimeSOTests()
    {
        Kulip.TimeSO time = ScriptableObject.CreateInstance<Kulip.TimeSO>();
        Assert.That(
            Mathf.Approximately(Time.time, time.Time),
            "TimeSO.Time == Time.time at start"
        );
        IEnumerator timeTest = TestTimeSOBehavior(time);
        while (timeTest.MoveNext())
        {
            yield return timeTest.Current;
        }
    }

    [UnityTest]
    public IEnumerator ScalableTimeSOTests()
    {
        float[] timeScales = new float[]{ 0.5f, 0.25f };

        Kulip.ScalableTimeSO time = ScriptableObject.CreateInstance<Kulip.ScalableTimeSO>();
        Assert.That(
            Mathf.Approximately(Time.time, time.Time),
            "ScalableTimeSO.Time == Time.time at start"
        );
        IEnumerator timeTest = TestTimeSOBehavior(time);
        while (timeTest.MoveNext())
        {
            yield return timeTest.Current;
        }

        IEnumerator TimeScalesCorrectly(float scale, int i)
        {
            time.Scale = scale;

            var startScaledTime = time.Time;
            var startTime = Time.time;

            yield return new WaitForSeconds(waitTime);

            var endScaledTime = time.Time;
            var endTime = Time.time;

            Assert.That(
                Mathf.Approximately(
                    (endTime - startTime) * scale,
                    endScaledTime - startScaledTime
                ),
                "ScalableTimeSO scales time properly for iter {0} with scale = {1}",
                i, scale
            );
        }

        foreach (var scale in timeScales) {
            IEnumerator scaleTest;
            scaleTest = TimeScalesCorrectly(scale, 0);
            while (scaleTest.MoveNext())
            {
                yield return scaleTest.Current;
            }
        }
    }


    IEnumerator TestTimeSOBehavior(Kulip.TimeSO time)
    {
        const float epsilon = 0.2f;

        bool pauseEventInvoked = false, resumeEventInvoked = false;
        time.PausedChangedEvent += p =>
        {
            if (p) pauseEventInvoked = true;
            else resumeEventInvoked = true;
        };

        var originalTime = time.Time;

        time.Pause();

        Assert.That(pauseEventInvoked, "TimeSO invokes PauseChangedEvent(true) after TimeSO.Pause()");

        yield return new WaitForSeconds(waitTime);

        Assert.That(
            Mathf.Approximately(originalTime, time.Time),
            "TimeSO.Time does not change when paused"
        );
        Assert.That(
            (Time.time - time.Time) - waitTime < epsilon
        );

        time.Resume();

        Assert.That(resumeEventInvoked, "TimeSO invokes PauseChangedEvent(false) after TimeSO.Resume()");

        yield return new WaitForSeconds(waitTime);

        Assert.That(
            (Time.time - time.Time) - waitTime < epsilon,
            "TimeSO resumes correctly."
        );

        time.Zero();
        Assert.That(
            Mathf.Approximately(time.Time, 0f),
            "TimeSO.Zero() sets TimeSO.Time = 0f"
        );
    }

}
