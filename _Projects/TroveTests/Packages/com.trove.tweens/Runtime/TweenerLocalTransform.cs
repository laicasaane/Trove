#if !DISABLE_UNITY_TRANSFORMS

using System;
using Unity.Mathematics;
using Unity.Transforms;

namespace Trove.Tweens
{
    [Serializable]
    public struct TweenerLocalTransform
    {
        public enum Type : byte
        {
            Position,
            PositionX,
            PositionY,
            PositionZ,

            Rotation,
            RotationEulerX,
            RotationEulerY,
            RotationEulerZ,

            UniformScale,
        }

        public enum Mode : byte
        {
            Absolute,
            Additive,
            AdditiveLocalSpace,
        }

        public float4 initial;
        public float4 target;
        public float4 internalAddedTarget;
        public Type tweenerType;
        public Mode tweenerMode;

        public static TweenerLocalTransform Position(in float3 target, Mode mode = Mode.Absolute)
        {
            return new TweenerLocalTransform {
                tweenerType = Type.Position,
                tweenerMode = mode,
                target = new float4(target.x, target.y, target.z, 0f),
            };
        }

        public static TweenerLocalTransform PositionX(float target, Mode mode = Mode.Absolute)
        {
            return new TweenerLocalTransform {
                tweenerType = Type.PositionX,
                tweenerMode = mode,
                target = new float4(target, 0f, 0f, 0f),
            };
        }

        public static TweenerLocalTransform PositionY(float target, Mode mode = Mode.Absolute)
        {
            return new TweenerLocalTransform {
                tweenerType = Type.PositionY,
                tweenerMode = mode,
                target = new float4(0f, target, 0f, 0f),
            };
        }

        public static TweenerLocalTransform PositionZ(float target, Mode mode = Mode.Absolute)
        {
            return new TweenerLocalTransform {
                tweenerType = Type.PositionZ,
                tweenerMode = mode,
                target = new float4(0f, 0f, target, 0f),
            };
        }

        public static TweenerLocalTransform Rotation(in quaternion target, Mode mode = Mode.Absolute)
        {
            return new TweenerLocalTransform {
                tweenerType = Type.Rotation,
                tweenerMode = mode,
                target = target.value,
            };
        }

        public static TweenerLocalTransform RotationEuler(in float3 target, Mode mode = Mode.Absolute)
        {
            return new TweenerLocalTransform {
                tweenerType = Type.Rotation,
                tweenerMode = mode,
                target = quaternion.Euler(target).value,
            };
        }

        public static TweenerLocalTransform RotationEulerX(float target, Mode mode = Mode.Absolute)
        {
            return new TweenerLocalTransform {
                tweenerType = Type.RotationEulerX,
                tweenerMode = mode,
                target = new float4(target, 0f, 0f, 0f),
            };
        }

        public static TweenerLocalTransform RotationEulerY(float target, Mode mode = Mode.Absolute)
        {
            return new TweenerLocalTransform {
                tweenerType = Type.RotationEulerY,
                tweenerMode = mode,
                target = new float4(0f, target, 0f, 0f),
            };
        }

        public static TweenerLocalTransform RotationEulerZ(float target, Mode mode = Mode.Absolute)
        {
            return new TweenerLocalTransform {
                tweenerType = Type.RotationEulerZ,
                tweenerMode = mode,
                target = new float4(0f, 0f, target, 0f),
            };
        }

        public static TweenerLocalTransform UniformScale(float target, bool isAdditive = false)
        {
            return new TweenerLocalTransform {
                tweenerType = Type.UniformScale,
                tweenerMode = isAdditive ? Mode.Additive : Mode.Absolute,
                target = new float4(target, 0f, 0f, 0f),
            };
        }

        public void StoreInitialValue(LocalTransform tweenedStruct)
        {
            if (tweenerMode == Mode.Absolute)
            {
                switch (tweenerType)
                {
                    case Type.Position:
                        initial = new float4(tweenedStruct.Position, 0f);
                        break;

                    case Type.PositionX:
                        initial.x = tweenedStruct.Position.x;
                        break;

                    case Type.PositionY:
                        initial.y = tweenedStruct.Position.y;
                        break;

                    case Type.PositionZ:
                        initial.z = tweenedStruct.Position.z;
                        break;

                    case Type.Rotation:
                        initial = tweenedStruct.Rotation.value;
                        break;

                    case Type.RotationEulerX:
                    case Type.RotationEulerY:
                    case Type.RotationEulerZ:
                        initial = new float4(tweenedStruct.Rotation.ToEuler(), 0f);
                        break;

                    case Type.UniformScale:
                        initial.x = tweenedStruct.Scale;
                        break;
                }
            }
            else
            {
                switch (tweenerType)
                {
                    case Type.Position:
                    case Type.PositionX:
                    case Type.PositionY:
                    case Type.PositionZ:
                    case Type.UniformScale:
                    case Type.RotationEulerX:
                    case Type.RotationEulerY:
                    case Type.RotationEulerZ:
                        internalAddedTarget = default;
                        initial = default;
                        break;

                    case Type.Rotation:
                        internalAddedTarget = quaternion.identity.value;
                        initial = quaternion.identity.value;
                        break;
                }
            }
        }

        public void Update(float ratio, ref LocalTransform tweenedStruct)
        {
            // Tween value from initial to target
            switch (tweenerType)
            {
                case Type.Position:
                    {
                        float3 initial = this.initial.ToFloat3();
                        float3 target = this.target.ToFloat3();
                        float3 ratioResult = math.lerp(initial, target, ratio);

                        switch (tweenerMode)
                        {
                            case Mode.Absolute:
                                {
                                    tweenedStruct.Position = ratioResult;
                                }
                                break;

                            case Mode.Additive:
                                {
                                    float3 added = ratioResult - internalAddedTarget.ToFloat3();
                                    internalAddedTarget = ratioResult.ToFloat4();
                                    tweenedStruct.Position += added;
                                }
                                break;

                            case Mode.AdditiveLocalSpace:
                                {
                                    float3 added = ratioResult - internalAddedTarget.ToFloat3();
                                    internalAddedTarget = ratioResult.ToFloat4();
                                    added = math.rotate(tweenedStruct.Rotation, added);
                                    tweenedStruct.Position += added;
                                }
                                break;
                        }
                    }
                    break;

                case Type.PositionX:
                    {
                        float initial = this.initial.x;
                        float target = this.target.x;
                        float ratioResult = math.lerp(initial, target, ratio);

                        switch (tweenerMode)
                        {
                            case Mode.Absolute:
                                {
                                    tweenedStruct.Position.x = ratioResult;
                                }
                                break;

                            case Mode.Additive:
                                {
                                    float added = ratioResult - internalAddedTarget.x;
                                    internalAddedTarget.x = ratioResult;
                                    tweenedStruct.Position.x += added;
                                }
                                break;

                            case Mode.AdditiveLocalSpace:
                                {
                                    float3 added = math.right() * (ratioResult - internalAddedTarget.x);
                                    internalAddedTarget.x = ratioResult;
                                    added = math.rotate(tweenedStruct.Rotation, added);
                                    tweenedStruct.Position += added;
                                }
                                break;
                        }
                    }
                    break;

                case Type.PositionY:
                    {
                        float initial = this.initial.y;
                        float target = this.target.y;
                        float ratioResult = math.lerp(initial, target, ratio);

                        switch (tweenerMode)
                        {
                            case Mode.Absolute:
                                {
                                    tweenedStruct.Position.y = ratioResult;
                                }
                                break;

                            case Mode.Additive:
                                {
                                    float added = ratioResult - internalAddedTarget.y;
                                    internalAddedTarget.y = ratioResult;
                                    tweenedStruct.Position.y += added;
                                }
                                break;

                            case Mode.AdditiveLocalSpace:
                                {
                                    float3 added = math.up() * (ratioResult - internalAddedTarget.y);
                                    internalAddedTarget.y = ratioResult;
                                    added = math.rotate(tweenedStruct.Rotation, added);
                                    tweenedStruct.Position += added;
                                }
                                break;
                        }
                    }
                    break;

                case Type.PositionZ:
                    {
                        float initial = this.initial.z;
                        float target = this.target.z;
                        float ratioResult = math.lerp(initial, target, ratio);

                        switch (tweenerMode)
                        {
                            case Mode.Absolute:
                                {
                                    tweenedStruct.Position.z = ratioResult;
                                }
                                break;

                            case Mode.Additive:
                                {
                                    float added = ratioResult - internalAddedTarget.z;
                                    internalAddedTarget.z = ratioResult;
                                    tweenedStruct.Position.z += added;
                                }
                                break;

                            case Mode.AdditiveLocalSpace:
                                {
                                    float3 added = math.forward() * (ratioResult - internalAddedTarget.z);
                                    internalAddedTarget.z = ratioResult;
                                    added = math.rotate(tweenedStruct.Rotation, added);
                                    tweenedStruct.Position += added;
                                }
                                break;
                        }
                    }
                    break;

                case Type.Rotation:
                    {
                        quaternion initial = new(this.initial);
                        quaternion target = new(this.target);

                        switch (tweenerMode)
                        {
                            case Mode.Absolute:
                                {
                                    tweenedStruct.Rotation = math.slerp(initial, target, ratio);
                                }
                                break;

                            case Mode.Additive:
                                {
                                    quaternion ratioResult = math.slerp(initial, target, ratio);
                                    quaternion added = math.mul(math.inverse(new quaternion(internalAddedTarget)), ratioResult);
                                    internalAddedTarget = ratioResult.value;
                                    tweenedStruct.Rotation = math.mul(added, tweenedStruct.Rotation);
                                }
                                break;

                            case Mode.AdditiveLocalSpace:
                                {
                                    quaternion ratioResult = math.slerp(initial, target, ratio);
                                    quaternion added = math.mul(math.inverse(ratioResult), new quaternion(internalAddedTarget));
                                    internalAddedTarget = ratioResult.value;
                                    tweenedStruct.Rotation = math.mul(tweenedStruct.Rotation, added);
                                }
                                break;
                        }
                    }
                    break;

                case Type.RotationEulerX:
                    {
                        float initial = this.initial.x;
                        float target = this.target.x;
                        float ratioResult = math.lerp(initial, target, ratio);

                        switch (tweenerMode)
                        {
                            case Mode.Absolute:
                                {
                                    float3 currentEuler = tweenedStruct.Rotation.ToEuler();
                                    tweenedStruct.Rotation = quaternion.Euler(ratioResult, currentEuler.y, currentEuler.z);
                                }
                                break;

                            case Mode.Additive:
                                {
                                    float added = ratioResult - internalAddedTarget.x;
                                    internalAddedTarget.x = ratioResult;
                                    tweenedStruct.Rotation = math.mul(quaternion.Euler(added, 0f, 0f), tweenedStruct.Rotation);
                                }
                                break;

                            case Mode.AdditiveLocalSpace:
                                {
                                    float added = ratioResult - internalAddedTarget.x;
                                    internalAddedTarget.x = ratioResult;
                                    tweenedStruct.Rotation = math.mul(tweenedStruct.Rotation, quaternion.Euler(added, 0f, 0f));
                                }
                                break;
                        }
                    }
                    break;

                case Type.RotationEulerY:
                    {
                        float initial = this.initial.y;
                        float target = this.target.y;
                        float ratioResult = math.lerp(initial, target, ratio);

                        switch (tweenerMode)
                        {
                            case Mode.Absolute:
                                {
                                    float3 currentEuler = tweenedStruct.Rotation.ToEuler();
                                    tweenedStruct.Rotation = quaternion.Euler(currentEuler.x, ratioResult, currentEuler.z);
                                }
                                break;

                            case Mode.Additive:
                                {
                                    float added = ratioResult - internalAddedTarget.y;
                                    internalAddedTarget.y = ratioResult;
                                    tweenedStruct.Rotation = math.mul(quaternion.Euler(0f, added, 0f), tweenedStruct.Rotation);
                                }
                                break;

                            case Mode.AdditiveLocalSpace:
                                {
                                    float added = ratioResult - internalAddedTarget.y;
                                    internalAddedTarget.y = ratioResult;
                                    tweenedStruct.Rotation = math.mul(tweenedStruct.Rotation, quaternion.Euler(0f, added, 0f));
                                }
                                break;
                        }
                    }
                    break;

                case Type.RotationEulerZ:
                    {
                        float initial = this.initial.z;
                        float target = this.target.z;
                        float ratioResult = math.lerp(initial, target, ratio);

                        switch (tweenerMode)
                        {
                            case Mode.Absolute:
                                {
                                    float3 currentEuler = tweenedStruct.Rotation.ToEuler();
                                    tweenedStruct.Rotation = quaternion.Euler(currentEuler.x, currentEuler.y, ratioResult);
                                }
                                break;

                            case Mode.Additive:
                                {
                                    float added = ratioResult - internalAddedTarget.z;
                                    internalAddedTarget.z = ratioResult;
                                    tweenedStruct.Rotation = math.mul(quaternion.Euler(0f, 0f, added), tweenedStruct.Rotation);
                                }
                                break;

                            case Mode.AdditiveLocalSpace:
                                {
                                    float added = ratioResult - internalAddedTarget.z;
                                    internalAddedTarget.z = ratioResult;
                                    tweenedStruct.Rotation = math.mul(tweenedStruct.Rotation, quaternion.Euler(0f, 0f, added));
                                }
                                break;
                        }
                    }
                    break;

                case Type.UniformScale:
                    {
                        float ratioResult = math.lerp(initial.x, target.x, ratio);

                        switch (tweenerMode)
                        {
                            case Mode.Absolute:
                                {
                                    tweenedStruct.Scale = ratioResult;
                                }
                                break;

                            case Mode.Additive:
                            case Mode.AdditiveLocalSpace:
                                {
                                    float addedScale = ratioResult - internalAddedTarget.x;
                                    internalAddedTarget.x = ratioResult;
                                    tweenedStruct.Scale += addedScale;
                                }
                                break;
                        }
                    }
                    break;
            }
        }
    }
}

#endif
