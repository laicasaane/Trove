using System;
using Unity.Mathematics;

namespace Trove.Tweens
{
    [Serializable]
    public struct TweenerFloat3
    {
        public EasingType easing;
        public float3 target;
        public InternalData<float3> internalData;

        public TweenerFloat3(
              in float3 target
            , bool targetIsRelative
            , EasingType easing = EasingType.Linear
        )
        {
            this.easing = easing;
            this.target = target;
            internalData = new(targetIsRelative);
        }

        public TweenerFloat3(
              in float3 initial
            , in float3 target
            , bool targetIsRelative
            , EasingType easing = EasingType.Linear
        )
        {
            this.easing = easing;
            this.target = target;
            internalData = new(initial, targetIsRelative);
        }

        public void Update(float normalizedTime, bool hasStartedPlaying, ref float3 value)
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
