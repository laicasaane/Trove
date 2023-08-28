using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Trove.Tweens
{
    [Serializable]
    public struct TweenTimer : IComponentData
    {
        public float Speed;

        public float __internal__duration;
        public float __internal__time;
        public float __internal__normalizedTime;
        public float __internal__excessTime;
        public ushort __internal__loopsCount;
        public byte __internal__flags;

        private const int HasChangedBitPosition = 0;
        private const int IsPlayingBitPosition = 1;
        private const int WasPlayingBitPosition = 2;
        private const int HasCompletedBitPosition = 3;
        private const int ProgressionDirectionBitPosition = 4;
        private const int IsLoopBitPosition = 5;
        private const int IsRewindBitPosition = 6;

        private bool InternalHasChanged { readonly get => BitUtilities.GetBit(__internal__flags, HasChangedBitPosition); set => BitUtilities.SetBit(value, ref __internal__flags, HasChangedBitPosition); }
        public bool IsPlaying { readonly get => BitUtilities.GetBit(__internal__flags, IsPlayingBitPosition); private set => BitUtilities.SetBit(value, ref __internal__flags, IsPlayingBitPosition); }
        private bool InternalWasPlaying { readonly get => BitUtilities.GetBit(__internal__flags, WasPlayingBitPosition); set => BitUtilities.SetBit(value, ref __internal__flags, WasPlayingBitPosition); }
        private bool InternalHasCompleted { readonly get => BitUtilities.GetBit(__internal__flags, HasCompletedBitPosition); set => BitUtilities.SetBit(value, ref __internal__flags, HasCompletedBitPosition); }
        public bool IsGoingInReverse { readonly get => BitUtilities.GetBit(__internal__flags, ProgressionDirectionBitPosition); private set => BitUtilities.SetBit(value, ref __internal__flags, ProgressionDirectionBitPosition); }
        public bool IsLoop { readonly get => BitUtilities.GetBit(__internal__flags, IsLoopBitPosition); set => BitUtilities.SetBit(value, ref __internal__flags, IsLoopBitPosition); }
        public bool IsRewind { readonly get => BitUtilities.GetBit(__internal__flags, IsRewindBitPosition); set => BitUtilities.SetBit(value, ref __internal__flags, IsRewindBitPosition); }

        public TweenTimer(in TweenAuthoringData data, bool autoPlay = false)
        {
            Speed = data.speed;

            __internal__duration = data.duration;
            __internal__time = 0f;
            __internal__normalizedTime = 0f;
            __internal__excessTime = 0f;
            __internal__loopsCount = 0;

            __internal__flags = 0;
            IsPlaying = false;
            InternalWasPlaying = false;
            InternalHasCompleted = false;
            IsGoingInReverse = false;
            IsLoop = data.isLoop;
            IsRewind = data.isRewind;

            ApplyChanges();

            if (autoPlay)
            {
                Play(true);
            }
        }

        public TweenTimer(float duration, bool isLoop, bool isRewind, float speed = 1f, bool autoPlay = false)
        {
            Speed = speed;

            __internal__duration = duration;
            __internal__time = 0f;
            __internal__normalizedTime = 0f;
            __internal__excessTime = 0f;
            __internal__loopsCount = 0;

            __internal__flags = 0;
            IsPlaying = false;
            InternalWasPlaying = false;
            InternalHasCompleted = false;
            IsGoingInReverse = false;
            IsLoop = isLoop;
            IsRewind = isRewind;

            ApplyChanges();

            if(autoPlay)
            {
                Play(true);
            }
        }

        public void Update(float deltaTime)
        {
            Update(deltaTime, out bool hasStartedPlaying, out bool hasStoppedPlaying, out bool hasChanged);
        }

        public void Update(float deltaTime, out bool hasChanged)
        {
            Update(deltaTime, out bool hasStartedPlaying, out bool hasStoppedPlaying, out hasChanged);
        }

        public void Update(float deltaTime, out bool hasStartedPlaying, out bool hasStoppedPlaying, out bool hasChanged)
        {
            hasStartedPlaying = false;
            hasStoppedPlaying = false;
            hasChanged = InternalHasChanged;

            if (IsPlaying)
            {
                InternalHasChanged = true;
                __internal__time += Speed * deltaTime * (IsGoingInReverse ? -1f : 1f);
                ApplyChanges();

                hasChanged = true;

                if (!InternalWasPlaying)
                {
                    hasStartedPlaying = true;
                }
            }
            else if (InternalWasPlaying)
            {
                hasStoppedPlaying = true;
            }

            InternalWasPlaying = IsPlaying;
            InternalHasChanged = false;
        }

        public readonly float GetTime()
        {
            return __internal__time;
        }

        public void SetTime(float value)
        {
            __internal__time = value;
            InternalHasChanged = true;
            ApplyChanges();
        }

        public readonly float GetDuration()
        {
            return __internal__duration;
        }

        public void SetDuration(float value)
        {
            __internal__duration = value;
            InternalHasChanged = true;
            ApplyChanges();
        }

        public readonly float GetNormalizedTime()
        {
            return __internal__normalizedTime;
        }

        public readonly float GetInverseNormalizedTime()
        {
            return 1f - __internal__normalizedTime;
        }

        public void SetNormalizedTime(float value)
        {
            __internal__time = value * __internal__duration;
            InternalHasChanged = true;
            ApplyChanges();
        }

        public readonly float GetExcessTime()
        {
            return __internal__excessTime;
        }

        public void Play(bool reset)
        {
            if (reset)
            {
                Stop();
            }
            IsPlaying = true;
        }

        public void Pause()
        {
            IsPlaying = false;
        }

        public void Stop()
        {
            ResetState();
            InternalHasChanged = true;
            SetTime(0f);
        }

        public void ResetState()
        {
            IsPlaying = false;
            InternalWasPlaying = false;
            InternalHasCompleted = false;
            IsGoingInReverse = false;
            InternalHasChanged = true;
            __internal__loopsCount = 0;
            ApplyChanges();
        }

        public bool HasCompleted()
        {
            return InternalHasCompleted;
        }

        public readonly int GetLoopsCount()
        {
            return __internal__loopsCount;
        }

        public void SetCourse(bool forward)
        {
            IsGoingInReverse = !forward;
            InternalHasChanged = true;
            ApplyChanges();
        }

        private void ApplyChanges()
        {
            InternalHasCompleted = false;

            if (__internal__duration > 0f)
            {
                // Reverse direction
                if (IsGoingInReverse)
                {
                    // Check reached completion
                    __internal__excessTime = -__internal__time;
                    if (__internal__excessTime > 0f)
                    {
                        if (IsRewind)
                        {
                            // Reached end of reverse progression
                            if (IsLoop)
                            {
                                IsGoingInReverse = false;
                                __internal__time = __internal__excessTime;
                                __internal__loopsCount++;
                            }
                            else
                            {
                                InternalHasCompleted = true;
                                IsPlaying = false;
                                IsGoingInReverse = false;
                                __internal__time = 0f;
                            }
                        }
                        else
                        {
                            InternalHasCompleted = true;
                            IsPlaying = false;
                            __internal__time = 0f;
                        }
                    }
                }
                // Forward direction
                else
                {
                    // Check reached completion
                    __internal__excessTime = __internal__time - __internal__duration;
                    if (__internal__excessTime > 0f)
                    {
                        if (IsRewind)
                        {
                            IsGoingInReverse = true;
                            __internal__time = __internal__duration - __internal__excessTime;
                        }
                        else
                        {
                            // Reached completion of forward progression
                            if (IsLoop)
                            {
                                __internal__time = __internal__excessTime;
                                __internal__loopsCount++;
                            }
                            else
                            {
                                InternalHasCompleted = true;
                                IsPlaying = false;
                                __internal__time = __internal__duration;
                            }
                        }
                    }
                }

                __internal__normalizedTime = math.saturate(__internal__time / __internal__duration);
            }
        }
    }
}
