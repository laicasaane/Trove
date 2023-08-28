using System;
using Unity.Mathematics;

namespace Trove.Tweens
{
    [Serializable]
    public struct TweenerHalf
    {
        public EasingType easing;
        public half target;
        public InternalData<half> internalData;
        
        public TweenerHalf(
              half target
            , bool targetIsRelative
            , EasingType easing = EasingType.Linear
        )
        {
            this.easing = easing;
            this.target = target;
            this.internalData = new(targetIsRelative);
        }

        public TweenerHalf(
              half initial
            , half target
            , bool targetIsRelative
            , EasingType easing = EasingType.Linear
        )
        {
            this.easing = easing;
            this.target = target;
            this.internalData = new(initial, targetIsRelative);
        }

        public void Update(half normalizedTime, bool hasStartedPlaying, ref half value)
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
            value = math.half(math.select(tweenedValue, internalData.initial + tweenedValue, internalData.TargetIsRelative));
        }
    }
}
