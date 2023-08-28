using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace Trove.Tweens
{
    public static unsafe class TweenUtilities
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UnityEngine.Color ToColor(in this float4 vec)
        {
            return new UnityEngine.Color(vec.x, vec.y, vec.z, vec.w);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 ToFloat4(in this UnityEngine.Color color)
        {
            return new float4(color.r, color.g, color.b, color.a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 ToFloat4(in this float3 f)
        {
            return new float4(f.x, f.y, f.z, 0f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 ToFloat3(in this float4 f)
        {
            return new float3(f.x, f.y, f.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ToByte(this bool value)
        {
            const byte TRUE = 1;
            const byte FALSE = 0;

            return value ? TRUE : FALSE;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ToBool(this byte value)
        {
            return value != 0;
        }

        // From Unity.Physics
        public static float3 ToEuler(this quaternion q, math.RotationOrder order = math.RotationOrder.XYZ)
        {
            const float epsilon = 1e-6f;

            var qv = q.value;
            var d1 = qv * qv.wwww * new float4(2.0f);
            var d2 = qv * qv.yzxw * new float4(2.0f);
            var d3 = qv * qv;
            var euler = new float3(0.0f);

            const float CUTOFF = (1.0f - 2.0f * epsilon) * (1.0f - 2.0f * epsilon);

            switch (order)
            {
                case math.RotationOrder.ZYX:
                    {
                        var y1 = d2.z + d1.y;
                        if (y1 * y1 < CUTOFF)
                        {
                            var x1 = -d2.x + d1.z;
                            var x2 = d3.x + d3.w - d3.y - d3.z;
                            var z1 = -d2.y + d1.x;
                            var z2 = d3.z + d3.w - d3.y - d3.x;
                            euler = new float3(math.atan2(x1, x2), math.asin(y1), math.atan2(z1, z2));
                        }
                        else
                        {
                            y1 = math.clamp(y1, -1.0f, 1.0f);
                            var abcd = new float4(d2.z, d1.y, d2.y, d1.x);
                            var x1 = 2.0f * (abcd.x * abcd.w + abcd.y * abcd.z); //2(ad+bc)
                            var x2 = math.csum(abcd * abcd * new float4(-1.0f, 1.0f, -1.0f, 1.0f));
                            euler = new float3(math.atan2(x1, x2), math.asin(y1), 0.0f);
                        }

                        break;
                    }

                case math.RotationOrder.ZXY:
                    {
                        var y1 = d2.y - d1.x;
                        if (y1 * y1 < CUTOFF)
                        {
                            var x1 = d2.x + d1.z;
                            var x2 = d3.y + d3.w - d3.x - d3.z;
                            var z1 = d2.z + d1.y;
                            var z2 = d3.z + d3.w - d3.x - d3.y;
                            euler = new float3(math.atan2(x1, x2), -math.asin(y1), math.atan2(z1, z2));
                        }
                        else
                        {
                            y1 = math.clamp(y1, -1.0f, 1.0f);
                            var abcd = new float4(d2.z, d1.y, d2.y, d1.x);
                            var x1 = 2.0f * (abcd.x * abcd.w + abcd.y * abcd.z);
                            var x2 = math.csum(abcd * abcd * new float4(-1.0f, 1.0f, -1.0f, 1.0f));
                            euler = new float3(math.atan2(x1, x2), -math.asin(y1), 0.0f);
                        }

                        break;
                    }

                case math.RotationOrder.YXZ:
                    {
                        var y1 = d2.y + d1.x;
                        if (y1 * y1 < CUTOFF)
                        {
                            var x1 = -d2.z + d1.y;
                            var x2 = d3.z + d3.w - d3.x - d3.y;
                            var z1 = -d2.x + d1.z;
                            var z2 = d3.y + d3.w - d3.z - d3.x;
                            euler = new float3(math.atan2(x1, x2), math.asin(y1), math.atan2(z1, z2));
                        }
                        else
                        {
                            y1 = math.clamp(y1, -1.0f, 1.0f);
                            var abcd = new float4(d2.x, d1.z, d2.y, d1.x);
                            var x1 = 2.0f * (abcd.x * abcd.w + abcd.y * abcd.z);
                            var x2 = math.csum(abcd * abcd * new float4(-1.0f, 1.0f, -1.0f, 1.0f));
                            euler = new float3(math.atan2(x1, x2), math.asin(y1), 0.0f);
                        }

                        break;
                    }

                case math.RotationOrder.YZX:
                    {
                        var y1 = d2.x - d1.z;
                        if (y1 * y1 < CUTOFF)
                        {
                            var x1 = d2.z + d1.y;
                            var x2 = d3.x + d3.w - d3.z - d3.y;
                            var z1 = d2.y + d1.x;
                            var z2 = d3.y + d3.w - d3.x - d3.z;
                            euler = new float3(math.atan2(x1, x2), -math.asin(y1), math.atan2(z1, z2));
                        }
                        else
                        {
                            y1 = math.clamp(y1, -1.0f, 1.0f);
                            var abcd = new float4(d2.x, d1.z, d2.y, d1.x);
                            var x1 = 2.0f * (abcd.x * abcd.w + abcd.y * abcd.z);
                            var x2 = math.csum(abcd * abcd * new float4(-1.0f, 1.0f, -1.0f, 1.0f));
                            euler = new float3(math.atan2(x1, x2), -math.asin(y1), 0.0f);
                        }

                        break;
                    }

                case math.RotationOrder.XZY:
                    {
                        var y1 = d2.x + d1.z;
                        if (y1 * y1 < CUTOFF)
                        {
                            var x1 = -d2.y + d1.x;
                            var x2 = d3.y + d3.w - d3.z - d3.x;
                            var z1 = -d2.z + d1.y;
                            var z2 = d3.x + d3.w - d3.y - d3.z;
                            euler = new float3(math.atan2(x1, x2), math.asin(y1), math.atan2(z1, z2));
                        }
                        else
                        {
                            y1 = math.clamp(y1, -1.0f, 1.0f);
                            var abcd = new float4(d2.x, d1.z, d2.z, d1.y);
                            var x1 = 2.0f * (abcd.x * abcd.w + abcd.y * abcd.z);
                            var x2 = math.csum(abcd * abcd * new float4(-1.0f, 1.0f, -1.0f, 1.0f));
                            euler = new float3(math.atan2(x1, x2), math.asin(y1), 0.0f);
                        }

                        break;
                    }

                case math.RotationOrder.XYZ:
                    {
                        var y1 = d2.z - d1.y;
                        if (y1 * y1 < CUTOFF)
                        {
                            var x1 = d2.y + d1.x;
                            var x2 = d3.z + d3.w - d3.y - d3.x;
                            var z1 = d2.x + d1.z;
                            var z2 = d3.x + d3.w - d3.y - d3.z;
                            euler = new float3(math.atan2(x1, x2), -math.asin(y1), math.atan2(z1, z2));
                        }
                        else
                        {
                            y1 = math.clamp(y1, -1.0f, 1.0f);
                            var abcd = new float4(d2.z, d1.y, d2.x, d1.z);
                            var x1 = 2.0f * (abcd.x * abcd.w + abcd.y * abcd.z);
                            var x2 = math.csum(abcd * abcd * new float4(-1.0f, 1.0f, -1.0f, 1.0f));
                            euler = new float3(math.atan2(x1, x2), -math.asin(y1), 0.0f);
                        }

                        break;
                    }
            }

            return order switch {
                math.RotationOrder.XZY => euler.xzy,
                math.RotationOrder.YZX => euler.zxy,
                math.RotationOrder.YXZ => euler.yxz,
                math.RotationOrder.ZXY => euler.yzx,
                math.RotationOrder.ZYX => euler.zyx,
                _ => euler,
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static quaternion Select(in quaternion a, in quaternion b, bool c)
        {
            if (!c)
            {
                return a;
            }

            return b;
        }

        public static void PlaySequence(
              bool reset
            , ref sbyte state
            , ref TweenTimer timer1
            , ref TweenTimer timer2
        )
        {
            int timersCount = 2;

            TweenTimer* timers = stackalloc TweenTimer[timersCount];
            timers[0] = timer1;
            timers[1] = timer2;

            PlaySequence(reset, ref state, timers, timersCount);

            timer1 = timers[0];
            timer2 = timers[1];
        }

        public static void PlaySequence(
              bool reset
            , ref sbyte state
            , ref TweenTimer timer1
            , ref TweenTimer timer2
            , ref TweenTimer timer3
        )
        {
            int timersCount = 3;

            TweenTimer* timers = stackalloc TweenTimer[timersCount];
            timers[0] = timer1;
            timers[1] = timer2;
            timers[2] = timer3;

            PlaySequence(reset, ref state, timers, timersCount);

            timer1 = timers[0];
            timer2 = timers[1];
            timer3 = timers[2];
        }

        public static void PlaySequence(
              bool reset
            , ref sbyte state
            , TweenTimer* timers
            , int timersCount
        )
        {
            if(timersCount <= 0)
                return;

            RefreshSequenceState(ref state, out int absoluteState, out int currentTimerIndex);

            if (reset)
            {
                state = 1;
                RefreshSequenceState(ref state, out absoluteState, out currentTimerIndex);

                for (int i = 1; i < timersCount; i++)
                {
                    TweenTimer otherTimer = timers[i];
                    otherTimer.Stop();
                    timers[i] = otherTimer;
                }
                TweenTimer timer = timers[currentTimerIndex];
                timer.Play(true);
                timers[currentTimerIndex] = timer;
            }
            else
            {
                TweenTimer timer = timers[currentTimerIndex];
                timer.Play(false);
                timers[currentTimerIndex] = timer;
            }
        }

        public static void SetSequenceCourse(
              bool forward
            , ref sbyte state
            , ref TweenTimer timer1
            , ref TweenTimer timer2
        )
        {
            int timersCount = 2;

            TweenTimer* timers = stackalloc TweenTimer[timersCount];
            timers[0] = timer1;
            timers[1] = timer2;

            SetSequenceCourse(forward, ref state, timers, timersCount);

            timer1 = timers[0];
            timer2 = timers[1];
        }

        public static void SetSequenceCourse(
              bool forward
            , ref sbyte state
            , ref TweenTimer timer1
            , ref TweenTimer timer2
            , ref TweenTimer timer3
        )
        {
            int timersCount = 3;

            TweenTimer* timers = stackalloc TweenTimer[timersCount];
            timers[0] = timer1;
            timers[1] = timer2;
            timers[2] = timer3;

            SetSequenceCourse(forward, ref state, timers, timersCount);

            timer1 = timers[0];
            timer2 = timers[1];
            timer3 = timers[2];
        }

        public static void SetSequenceCourse(
              bool forward
            , ref sbyte state
            , TweenTimer* timers
            , int timersCount
        )
        {
            if (timersCount <= 0)
                return;

            RefreshSequenceState(ref state, out int absoluteState, out int currentTimerIndex);

            if (forward)
            {
                // Invert state if we were in reverse
                if(state < 0)
                {
                    state = (sbyte)(-state);
                    RefreshSequenceState(ref state, out absoluteState, out currentTimerIndex);
                }
                TweenTimer timer = timers[currentTimerIndex];
                timer.SetCourse(true);
                timers[currentTimerIndex] = timer;
            }
            else
            {
                // Invert state if we were in forward
                if (state > 0)
                {
                    state = (sbyte)(-state);
                    RefreshSequenceState(ref state, out absoluteState, out currentTimerIndex);
                }
                TweenTimer timer = timers[currentTimerIndex];
                timer.SetCourse(false);
                timers[currentTimerIndex] = timer;
            }
        }

        public static void UpdateSequence(
              ref sbyte state
            , ref TweenTimer timer1
            , ref TweenTimer timer2
        )
        {
            int timersCount = 2;

            TweenTimer* timers = stackalloc TweenTimer[timersCount];
            timers[0] = timer1;
            timers[1] = timer2;

            UpdateSequence(ref state, timers, timersCount);

            timer1 = timers[0];
            timer2 = timers[1];
        }

        public static void UpdateSequence(
              ref sbyte state
            , ref TweenTimer timer1
            , ref TweenTimer timer2
            , ref TweenTimer timer3
        )
        {
            int timersCount = 3;

            TweenTimer* timers = stackalloc TweenTimer[timersCount];
            timers[0] = timer1;
            timers[1] = timer2;
            timers[2] = timer3;

            UpdateSequence(ref state, timers, timersCount);

            timer1 = timers[0];
            timer2 = timers[1];
            timer3 = timers[2];
        }

        public static void UpdateSequence(
              ref sbyte state
            , TweenTimer* timers
            , int timersCount
        )
        {
            if (timersCount <= 0)
                return;

            RefreshSequenceState(ref state, out int absoluteState, out int currentTimerIndex);

            if (timers[currentTimerIndex].HasCompleted())
            {
                TweenTimer prevTimer = timers[currentTimerIndex];

                // Detect starting next timer in forward sequence
                if (state > 0 && state < timersCount)
                {
                    state++;
                    RefreshSequenceState(ref state, out absoluteState, out currentTimerIndex);
                    TweenTimer newTimer = timers[currentTimerIndex];
                    newTimer.SetCourse(true);
                    newTimer.SetTime(prevTimer.GetExcessTime());
                    newTimer.Play(false);
                    timers[currentTimerIndex] = newTimer;
                }
                // Detect starting next timer in reverse sequence
                else if (state < 0 && state < -1)
                {
                    state++;
                    RefreshSequenceState(ref state, out absoluteState, out currentTimerIndex);
                    TweenTimer newTimer = timers[currentTimerIndex];
                    newTimer.SetCourse(false);
                    newTimer.SetTime(newTimer.GetDuration() - prevTimer.GetExcessTime());
                    newTimer.Play(false);
                    timers[currentTimerIndex] = newTimer;
                }
            }
        }

        private static void RefreshSequenceState(
              ref sbyte state
            , out int absoluteState
            , out int currentTimerIndex
        )
        {
            if (state == 0)
            {
                state = 1;
            }

            absoluteState = math.abs(state);
            currentTimerIndex = (sbyte)(absoluteState - 1);
        }
    }
}
