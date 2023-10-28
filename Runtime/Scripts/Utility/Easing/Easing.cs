using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Kulip
{
    public static class Ease
    {
        /**
         * CREDIT: https://easings.net/
         * See License: https://github.com/ai/easings.net/blob/master/LICENSE
         */

        /* Utility */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double InToOut(System.Func<double, double> f, double t) => 1 - f(1 - t);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float InToOut(System.Func<float, float> f, float t) => 1 - f(1 - t);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double InToInOut(System.Func<double, double> f, double t) => t < 0.5 ? f(2 * t) / 2 : 1 - f(2 * (1 - t)) / 2;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float InToInOut(System.Func<float, float> f, float t) => t < 0.5f ? f(2 * t) / 2 : 1 - f(2 * (1 - t)) / 2;

        public static double Linear(double t) => t;
        public static float Linear(float t) => t;

        // https://easings.net/#easeOutSine
        public static double OutSine(double t) => System.Math.Sin((t * System.Math.PI) / 2);
        public static float OutSine(float t) => System.MathF.Sin((t * System.MathF.PI) / 2);

        // Equivalent to: 1 - Math.cos((x * Math.PI) / 2);
        // https://easings.net/#easeInSine
        public static double InSine(double t) => InToOut(OutSine, t);
        public static float InSine(float t) => InToOut(OutSine, t);


        // https://easings.net/#easeInOutSine
        public static double InOutSine(double t) => -(System.Math.Cos(t * System.Math.PI) - 1) / 2;
        public static float InOutSine(float t) => -(System.MathF.Cos(t * System.MathF.PI) - 1) / 2;

        // https://easings.net/#easeInQuad
        public static double InQuad(double t) => t * t;
        public static float InQuad(float t) => t * t;

        // https://easings.net/#easeOutQuad
        public static double OutQuad(double t) => InToOut(InQuad, t);
        public static float OutQuad(float t) => InToOut(InQuad, t);

        // https://easings.net/#easeInOutQuad
        public static double InOutQuad(double t) => InToInOut(InQuad, t);
        public static float InOutQuad(float t) => InToInOut(InQuad, t);


        // https://easings.net/#easeInCubic
        public static double InCubic(double t) => t * t * t;
        public static float InCubic(float t) => t * t * t;


        // https://easings.net/#easeOutCubic
        public static double OutCubic(double t) => InToOut(InCubic, t);
        public static float OutCubic(float t) => InToOut(InCubic, t);

        // https://easings.net/#easeInOutCubic
        public static double InOutCubic(double t) => InToInOut(InCubic, t);
        public static float InOutCubic(float t) => InToInOut(InCubic, t);

        // https://easings.net/#easeInQuart
        public static double InQuart(double t) => t * t * t * t;
        public static float InQuart(float t) => t * t;

        // https://easings.net/#easeOutQuart
        public static double OutQuart(double t) => InToOut(InQuart, t);
        public static float OutQuart(float t) => InToOut(InQuart, t);

        // https://easings.net/#easeInOutQuart
        public static double InOutQuart(double t) => InToInOut(InQuart, t);
        public static float InOutQuart(float t) => InToInOut(InQuart, t);

        // https://easings.net/#easeInQuint
        public static double InQuint(double t) => t * t * t * t * t;
        public static float InQuint(float t) => t * t * t * t * t;

        // https://easings.net/#easeOutQuint
        public static double OutQuint(double t) => InToOut(InQuint, t);
        public static float OutQuint(float t) => InToOut(InQuint, t);

        // https://easings.net/#easeInOutQuint
        public static double InOutQuint(double t) => InToInOut(InQuint, t);
        public static float InOutQuint(float t) => InToInOut(InQuint, t);


        // https://easings.net/#easeInExpo
        public static double InExpo(double t) => t == 0 ? 0 : System.Math.Pow(2, 10 * t - 10);
        public static float InExpo(float t) => t == 0 ? 0 : System.MathF.Pow(2, 10 * t - 10);

        // https://easings.net/#easeOutExpo
        public static double OutExpo(double t) => InToOut(InExpo, t);
        public static float OutExpo(float t) => InToOut(InExpo, t);

        // https://easings.net/#easeInOutExpo
        public static double InOutExpo(double t) => InToInOut(InExpo, t);
        public static float InOutExpo(float t) => InToInOut(InExpo, t);


        // https://easings.net/#easeInCirc
        public static double InCirc(double t) => 1 - System.Math.Sqrt(1 - t * t);
        public static float InCirc(float t) => 1 - System.MathF.Sqrt(1 - t * t);

        // https://easings.net/#easeOutCirc
        public static double OutCirc(double t) => InToOut(InCirc, t);
        public static float OutCirc(float t) => InToOut(InCirc, t);

        // https://easings.net/#easeInOutCirc
        public static double InOutCirc(double t) => InToInOut(InCirc, t);
        public static float InOutCirc(float t) => InToInOut(InCirc, t);

        // https://easings.net/#easeInBack
        public static double InBack(double t) => 2.70158 * t * t * t - 1.70158 * t * t;
        public static float InBack(float t) => 2.70158f * t * t * t - 1.70158f * t * t;

        // https://easings.net/#easeOutBack
        public static double OutBack(double t) => InToOut(InBack, t);
        public static float OutBack(float t) => InToOut(InBack, t);

        // https://easings.net/#easeInOutBack
        public static double InOutBack(double t) => InToInOut(InBack, t);
        public static float InOutBack(float t) => InToInOut(InBack, t);

        // https://easings.net/#easeInElastic
        public static double InElastic(double t) => -System.Math.Pow(2.0, 10 * t - 10) * System.Math.Sin((10 * t - 10.75) * System.Math.PI * 2.0 / 3);
        public static float InElastic(float t) => -System.MathF.Pow(2.0f, 10 * t - 10) * System.MathF.Sin((10 * t - 10.75f) * System.MathF.PI * 2.0f / 3);

        // https://easings.net/#easeOutElastic
        public static double OutElastic(double t) => InToOut(InElastic, t);
        public static float OutElastic(float t) => InToOut(InElastic, t);

        // https://easings.net/#easeInOutElastic
        public static double InOutElastic(double t) => InToInOut(InElastic, t);
        public static float InOutElastic(float t) => InToInOut(InElastic, t);

        // https://easings.net/#easeInBounce
        public static double InBounce(double t) => InToOut(OutBounce, t);
        public static float InBounce(float t) => InToOut(OutBounce, t);

        // https://easings.net/#easeOutBounce
        const double n1 = 7.5625;
        const double d1 = 2.75;
        const float n1f = 7.5625f;
        const float d1f = 2.75f;

        public static double OutBounce(double t)
            => t < 1 / d1
            ? n1 * t * t
            : t < 2 / d1
            ? n1 * (t - 1.5 / d1) * (t - 1.5 / d1) + 0.75
            : t < 2.5 / d1
            ? n1 * (t - 2.25 / d1) * (t - 2.25 / d1) + 0.9375
            : n1 * (t - 2.625 / d1) * (t - 2.625 / d1) + 0.984375;
        public static float OutBounce(float t)
            => t < 1 / d1f
            ? n1f * t * t
            : t < 2 / d1f
            ? n1f * (t - 1.5f / d1f) * (t - 1.5f / d1f) + 0.75f
            : t < 2.5f / d1f
            ? n1f * (t - 2.25f / d1f) * (t - 2.25f / d1f) + 0.9375f
            : n1f * (t - 2.625f / d1f) * (t - 2.625f / d1f) + 0.984375f;

        // https://easings.net/#easeInOutBounce
        public static double InOutBounce(double t) => InToInOut(InBounce, t);
        public static float InOutBounce(float t) => InToInOut(InBounce, t);
    }
}
