using System;
using Unity.Mathematics;
using Unity.Transforms;

namespace Trove.Tweens
{
    [Serializable]
    public struct TweenerScale
    {
        public enum Type : byte
        {
            Scale,
            ScaleX,
            ScaleY,
            ScaleZ,
        }

        public float3 initial;
        public float3 target;
        public float3 internalAddedTarget;
        public Type tweenerType;
        public byte additive;

        public static TweenerScale Scale(float3 target, bool additive = false)
        {
            return new TweenerScale {
                tweenerType = Type.Scale,
                additive = additive.ToByte(),
                target = target,
            };
        }

        public static TweenerScale ScaleX(float target, bool additive = false)
        {
            return new TweenerScale {
                tweenerType = Type.ScaleX,
                additive = additive.ToByte(),
                target = new float3(target, 0f, 0f),
            };
        }

        public static TweenerScale ScaleY(float target, bool additive = false)
        {
            return new TweenerScale {
                tweenerType = Type.ScaleY,
                additive = additive.ToByte(),
                target = new float3(0f, target, 0f),
            };
        }

        public static TweenerScale ScaleZ(float target, bool additive = false)
        {
            return new TweenerScale {
                tweenerType = Type.ScaleZ,
                additive = additive.ToByte(),
                target = new float3(0f, 0f, target),
            };
        }

        public void StoreInitialValue(in PostTransformMatrix tweenedStruct)
        {
            if (additive.ToBool())
            {
                internalAddedTarget = default;
                initial = default;
            }
            else
            {
                switch (tweenerType)
                {
                    case Type.Scale:
                        initial = tweenedStruct.Value.Scale();
                        break;
                    case Type.ScaleX:
                        initial.x = tweenedStruct.Value.Scale().x;
                        break;
                    case Type.ScaleY:
                        initial.y = tweenedStruct.Value.Scale().y;
                        break;
                    case Type.ScaleZ:
                        initial.z = tweenedStruct.Value.Scale().z;
                        break;
                }
            }
        }

        public void Update(float ratio, ref PostTransformMatrix tweenedStruct)
        {
            // Tween value from initial to target
            switch (tweenerType)
            {
                case Type.Scale:
                    {
                        float3 initial = new(this.initial.x, this.initial.y, this.initial.z);
                        float3 target = new(this.target.x, this.target.y, this.target.z);
                        float3 ratioResult = math.lerp(initial, target, ratio);

                        if (additive.ToBool())
                        {
                            float3 addedScale = ratioResult - internalAddedTarget.x;
                            internalAddedTarget = ratioResult;
                            float3 newScale = tweenedStruct.Value.Scale() + addedScale;
                            tweenedStruct.Value = float4x4.Scale(newScale);

                        }
                        else
                        {
                            tweenedStruct.Value = float4x4.Scale(ratioResult);
                        }
                    }
                    break;

                case Type.ScaleX:
                    {
                        float initial = this.initial.x;
                        float target = this.target.x;
                        float ratioResult = math.lerp(initial, target, ratio);

                        if (additive.ToBool())
                        {
                            float addedScale = ratioResult - internalAddedTarget.x;
                            internalAddedTarget = ratioResult;
                            float3 newScale = tweenedStruct.Value.Scale() + new float3(addedScale, 0f, 0f);
                            tweenedStruct.Value = float4x4.Scale(newScale);

                        }
                        else
                        {
                            float3 currentScale = tweenedStruct.Value.Scale();
                            float3 newScale = new(ratioResult, currentScale.y, currentScale.z);
                            tweenedStruct.Value = float4x4.Scale(newScale);
                        }
                    }
                    break;

                case Type.ScaleY:
                    {
                        float initial = this.initial.y;
                        float target = this.target.y;
                        float ratioResult = math.lerp(initial, target, ratio);

                        if (additive.ToBool())
                        {
                            float addedScale = ratioResult - internalAddedTarget.y;
                            internalAddedTarget = ratioResult;
                            float3 newScale = tweenedStruct.Value.Scale() + new float3(0f, addedScale, 0f);
                            tweenedStruct.Value = float4x4.Scale(newScale);
                        }
                        else
                        {
                            float3 currentScale = tweenedStruct.Value.Scale();
                            float3 newScale = new(currentScale.x, ratioResult, currentScale.z);
                            tweenedStruct.Value = float4x4.Scale(newScale);
                        }
                    }
                    break;

                case Type.ScaleZ:
                    {
                        float initial = this.initial.z;
                        float target = this.target.z;
                        float ratioResult = math.lerp(initial, target, ratio);

                        if (additive.ToBool())
                        {
                            float addedScale = ratioResult - internalAddedTarget.z;
                            internalAddedTarget = ratioResult;
                            float3 newScale = tweenedStruct.Value.Scale() + new float3(0f, 0f, addedScale);
                            tweenedStruct.Value = float4x4.Scale(newScale);
                        }
                        else
                        {
                            float3 currentScale = tweenedStruct.Value.Scale();
                            float3 newScale = new(currentScale.x, currentScale.y, ratioResult);
                            tweenedStruct.Value = float4x4.Scale(newScale);
                        }
                    }
                    break;
            }
        }
    }
}
