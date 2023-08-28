using System;
using Unity.Mathematics;

namespace Trove.Tweens
{
    [Serializable]
    public struct TweenerFloat
    {
        public EasingType easing;
        public float target;
        public InternalData<float> internalData;
        
        public TweenerFloat(
              float target
            , bool targetIsRelative
            , EasingType easing = EasingType.Linear
        )
        {
            this.easing = easing;
            this.target = target;
            this.internalData = new(targetIsRelative);
        }

        public TweenerFloat(
              float initial
            , float target
            , bool targetIsRelative
            , EasingType easing = EasingType.Linear
        )
        {
            this.easing = easing;
            this.target = target;
            this.internalData = new(initial, targetIsRelative);
        }

        public void Update(float normalizedTime, bool hasStartedPlaying, ref float value)
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
            value = math.select(tweenedValue, internalData.initial + tweenedValue, internalData.TargetIsRelative);
        }
    }
}
