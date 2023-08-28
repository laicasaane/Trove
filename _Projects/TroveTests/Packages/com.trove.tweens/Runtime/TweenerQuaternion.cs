using System;
using Unity.Mathematics;

namespace Trove.Tweens
{
    [Serializable]
    public struct TweenerQuaternion
    {
        public EasingType easing;
        public quaternion target;
        public InternalData<quaternion> internalData;

        public TweenerQuaternion(
              in quaternion target
            , bool targetIsRelative
            , EasingType easing = EasingType.Linear
        )
        {
            this.easing = easing;
            this.target = target;
            this.internalData = new(targetIsRelative);
        }

        public TweenerQuaternion(
              in quaternion initial
            , in quaternion target
            , bool targetIsRelative
            , EasingType easing = EasingType.Linear
        )
        {
            this.easing = easing;
            this.target = target;
            this.internalData = new(initial, targetIsRelative);
        }

        public void Update(float normalizedTime, bool hasStartedPlaying, ref quaternion value)
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

            var tweenedValue = math.slerp(internalData.initialTweened, target, Easing.Ease(normalizedTime, easing));
            value = TweenUtilities.Select(
                  tweenedValue
                , math.mul(tweenedValue, internalData.initial)
                , internalData.TargetIsRelative
            );
        }
    }
}
