using System;
using Unity.Mathematics;

namespace Trove.Tweens
{
    [Serializable]
    public struct TweenerInt
    {
        public EasingType easing;
        public int target;
        public InternalData<int> internalData;
        
        public TweenerInt(
              int target
            , bool targetIsRelative
            , EasingType easing = EasingType.Linear
        )
        {
            this.easing = easing;
            this.target = target;
            this.internalData = new(targetIsRelative);
        }

        public TweenerInt(
              int initial
            , int target
            , bool targetIsRelative
            , EasingType easing = EasingType.Linear
        )
        {
            this.easing = easing;
            this.target = target;
            this.internalData = new(initial, targetIsRelative);
        }

        public void Update(int normalizedTime, bool hasStartedPlaying, ref int value)
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

            var tweenedValue = (int)math.lerp(internalData.initialTweened, target, Easing.Ease(normalizedTime, easing));
            value = math.select(tweenedValue, internalData.initial + tweenedValue, internalData.TargetIsRelative);
        }
    }
}
