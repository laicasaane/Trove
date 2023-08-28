using System;
using Unity.Mathematics;

namespace Trove.Tweens
{
    [Serializable]
    public struct TweenerFloat4
    {
        public EasingType easing;
        public float4 target;
        public InternalData<float4> internalData;

        public TweenerFloat4(
              in float4 target
            , bool targetIsRelative
            , EasingType easing = EasingType.Linear
        )
        {
            this.easing = easing;
            this.target = target;
            internalData = new(targetIsRelative);
        }

        public TweenerFloat4(
              in float4 initial
            , in float4 target
            , bool targetIsRelative
            , EasingType easing = EasingType.Linear
        )
        {
            this.easing = easing;
            this.target = target;
            internalData = new(initial, targetIsRelative);
        }

        public void Update(float normalizedTime, bool hasStartedPlaying, ref float4 value)
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
