using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace Trove.Tweens
{
    // Credit goes to https://easings.net/
    public static class Easing
    {
        private const float C1 = 1.70158f;
        private const float C2 = C1 * 1.525f;
        private const float C3 = C1 + 1f;
        private const float C4 = (2f * math.PI) / 3f;
        private const float C5 = (2f * math.PI) / 4.5f;
        private const float N1 = 7.5625f;
        private const float D1 = 2.75f;

        public static float Ease(float x, EasingType type)
        {
            return type switch {
                EasingType.None => 0f,
                EasingType.Linear => EaseLinear(x),
                EasingType.EaseInSine => EaseInSine(x),
                EasingType.EaseOutSine => EaseOutSine(x),
                EasingType.EaseInOutSine => EaseInOutSine(x),
                EasingType.EaseInQuad => EaseInQuad(x),
                EasingType.EaseOutQuad => EaseOutQuad(x),
                EasingType.EaseInOutQuad => EaseInOutQuad(x),
                EasingType.EaseInCubic => EaseInCubic(x),
                EasingType.EaseOutCubic => EaseOutCubic(x),
                EasingType.EaseInOutCubic => EaseInOutCubic(x),
                EasingType.EaseInQuart => EaseInQuart(x),
                EasingType.EaseOutQuart => EaseOutQuart(x),
                EasingType.EaseInOutQuart => EaseInOutQuart(x),
                EasingType.EaseInQuint => EaseInQuint(x),
                EasingType.EaseOutQuint => EaseOutQuint(x),
                EasingType.EaseInOutQuint => EaseInOutQuint(x),
                EasingType.EaseInExpo => EaseInExpo(x),
                EasingType.EaseOutExpo => EaseOutExpo(x),
                EasingType.EaseInOutExpo => EaseInOutExpo(x),
                EasingType.EaseInCirc => EaseInCirc(x),
                EasingType.EaseOutCirc => EaseOutCirc(x),
                EasingType.EaseInOutCirc => EaseInOutCirc(x),
                EasingType.EaseInBack => EaseInBack(x),
                EasingType.EaseOutBack => EaseOutBack(x),
                EasingType.EaseInOutBack => EaseInOutBack(x),
                EasingType.EaseInElastic => EaseInElastic(x),
                EasingType.EaseOutElastic => EaseOutElastic(x),
                EasingType.EaseInOutElastic => EaseInOutElastic(x),
                EasingType.EaseInBounce => EaseInBounce(x),
                EasingType.EaseOutBounce => EaseOutBounce(x),
                EasingType.EaseInOutBounce => EaseInOutBounce(x),
                _ => 0f,
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseLinear(float x)
            => x;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInSine(float x)
            => 1f - math.cos((x * math.PI) / 2f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOutSine(float x)
            => math.sin((x * math.PI) / 2f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOutSine(float x)
            => -(math.cos(x * math.PI) - 1f) / 2f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInQuad(float x)
            => x * x;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOutQuad(float x)
            => 1f - (1f - x) * (1f - x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOutQuad(float x)
            => math.select(1f - math.pow((-2f * x) + 2f, 2f) / 2f, 2f * x * x, x < 0.5f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInCubic(float x)
            => x * x * x;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOutCubic(float x)
            => 1f - math.pow(1f - x, 3f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOutCubic(float x)
            => math.select(1f - math.pow((-2f * x) + 2f, 3f) / 2f, 4f * x * x * x, x < 0.5f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInQuart(float x)
            => x * x * x * x;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOutQuart(float x)
            => 1f - math.pow(1f - x, 4f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOutQuart(float x)
            => math.select(1f - math.pow((-2f * x) + 2f, 4f) / 2f, 8f * x * x * x * x, x < 0.5f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInQuint(float x)
            => x * x * x * x * x;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOutQuint(float x)
            => 1f - math.pow(1f - x, 5f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOutQuint(float x)
            => math.select(1f - math.pow((-2f * x) + 2f, 5f) / 2f, 16f * x * x * x * x * x, x < 0.5f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInExpo(float x)
            => math.select(math.pow(2f, (10f * x) - 10f), 0f, x == 0f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOutExpo(float x)
            => math.select(1f - math.pow(2f, -10f * x), 1f, x == 1f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOutExpo(float x)
            => math.select(math.select(math.select((2f - math.pow(2f, (-20f * x) + 10f)) / 2, math.pow(2f, (20f * x) - 10f) / 2f, x < 0.5f), 1f, x == 1f), 0f, x == 0f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInCirc(float x)
            => 1f - math.sqrt(1f - math.pow(x, 2f));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOutCirc(float x)
            => math.sqrt(1f - math.pow(x - 1f, 2f));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOutCirc(float x)
            => math.select((math.sqrt(1f - math.pow((-2f * x) + 2f, 2f)) + 1f) / 2f, (1f - math.sqrt(1f - math.pow(2f * x, 2f))) / 2f, x < 0.5f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInBack(float x)
            => (C3 * x * x * x) - (C1 * x * x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOutBack(float x)
            => 1f + (C3 * math.pow(x - 1f, 3f)) + (C1 * math.pow(x - 1f, 2f));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOutBack(float x)
            => math.select((math.pow((2f * x) - 2f, 2f) * ((C2 + 1f) * ((x * 2f) - 2f) + C2) + 2f) / 2f, (math.pow(2f * x, 2f) * (((C2 + 1f) * 2f * x) - C2)) / 2f, x < 0.5f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInElastic(float x)
            => math.select(math.select(-math.pow(2f, (10f * x) - 10f) * math.sin(((x * 10f) - 10.75f) * C4), 1f, x == 1f), 0f, x == 0f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOutElastic(float x)
            => math.select(math.select(math.pow(2f, -10f * x) * math.sin(((x * 10f) - 0.75f) * C4) + 1f, 1f, x == 1f), 0f, x == 0f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOutElastic(float x)
            => math.select(math.select(math.select(((math.pow(2f, (-20f * x) + 10f) * math.sin(((20f * x) - 11.125f) * C5)) / 2f) + 1f, -((math.pow(2f, (20f * x) - 10f) * math.sin(((20f * x) - 11.125f) * C5)) / 2f), x < 0.5f), 1f, x == 1f), 0f, x == 0f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInBounce(float x)
            => 1f - EaseOutBounce(1f - x);

        public static float EaseOutBounce(float x)
        {
            if (x < (1f / D1))
            {
                return N1 * x * x;
            }
            else if (x < (2f / D1))
            {
                return (N1 * (x -= (1.5f / D1)) * x) + 0.75f;
            }
            else if (x < (2.5f / D1))
            {
                return (N1 * (x -= (2.25f / D1)) * x) + 0.9375f;
            }
            else
            {
                return (N1 * (x -= (2.625f / D1)) * x) + 0.984375f;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOutBounce(float x)
            => math.select((1f + EaseOutBounce((2f * x) - 1f)) / 2f, (1f - EaseOutBounce(1f - (2f * x))) / 2f, x < 0.5f);
    }
}
