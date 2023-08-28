using System;
using Unity.Mathematics;

namespace Trove.Tweens
{
    [Serializable]
    public struct TweenerHalf2
    {
        public EasingType easing;
        public half2 target;
        public InternalData<half2> internalData;

        public TweenerHalf2(
              half2 target
            , bool targetIsRelative
            , EasingType easing = EasingType.Linear
        )
        {
            this.easing = easing;
            this.target = target;
            internalData = new(targetIsRelative);
        }

        public TweenerHalf2(
              half2 initial
            , half2 target
            , bool targetIsRelative
            , EasingType easing = EasingType.Linear
        )
        {
            this.easing = easing;
            this.target = target;
            internalData = new(initial, targetIsRelative);
        }

        public void Update(half normalizedTime, bool hasStartedPlaying, ref half2 value)
        {
            if (hasStartedPlaying)
            {
                if (internalData.CaptureInitialOnStartPlaying)
                {
                    internalData.initial = value;
                }

                if (internalData.TargetIsRelative)
                {
                    internalData.initialTweened = default;
                }
            }

            var tweenedValue = math.lerp(internalData.initialTweened, target, Easing.Ease(normalizedTime, easing));
            value = math.half2(math.select(tweenedValue, internalData.initial + tweenedValue, internalData.TargetIsRelative));
        }
    }
}
