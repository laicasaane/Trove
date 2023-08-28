using System;
using Unity.Mathematics;

namespace Trove.Tweens
{
    [Serializable]
    public struct TweenerInt3
    {
        public EasingType easing;
        public int3 target;
        public InternalData<int3> internalData;

        public TweenerInt3(
              in int3 target
            , bool targetIsRelative
            , EasingType easing = EasingType.Linear
        )
        {
            this.easing = easing;
            this.target = target;
            internalData = new(targetIsRelative);
        }

        public TweenerInt3(
              in int3 initial
            , in int3 target
            , bool targetIsRelative
            , EasingType easing = EasingType.Linear
        )
        {
            this.easing = easing;
            this.target = target;
            internalData = new(initial, targetIsRelative);
        }

        public void Update(int normalizedTime, bool hasStartedPlaying, ref int3 value)
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

            var tweenedValue = (int3)math.lerp(internalData.initialTweened, target, Easing.Ease(normalizedTime, easing));
            value = math.select(tweenedValue, internalData.initial + tweenedValue, internalData.TargetIsRelative);
        }
    }
}
