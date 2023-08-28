using System;
using Unity.Mathematics;

namespace Trove.Tweens
{
    [Serializable]
    public struct TweenerHalf3
    {
        public EasingType easing;
        public half3 target;
        public InternalData<half3> internalData;

        public TweenerHalf3(
              half3 target
            , bool targetIsRelative
            , EasingType easing = EasingType.Linear
        )
        {
            this.easing = easing;
            this.target = target;
            internalData = new(targetIsRelative);
        }

        public TweenerHalf3(
              half3 initial
            , half3 target
            , bool targetIsRelative
            , EasingType easing = EasingType.Linear
        )
        {
            this.easing = easing;
            this.target = target;
            internalData = new(initial, targetIsRelative);
        }

        public void Update(half normalizedTime, bool hasStartedPlaying, ref half3 value)
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
            value = math.half3(math.select(tweenedValue, internalData.initial + tweenedValue, internalData.TargetIsRelative));
        }
    }
}
