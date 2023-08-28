using System;
using System.Runtime.CompilerServices;

namespace Trove.Tweens
{
    [Serializable]
    public struct InternalData<T> where T : unmanaged
    {
        private const int CAPTURE_INITIAL_ON_START_PLAYING_BIT_POSITION = 0;
        private const int TARGET_IS_RELATIVE_BIT_POSITION = 1;

        public byte flags;
        public T initial;
        public T initialTweened;

        public bool CaptureInitialOnStartPlaying
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => BitUtilities.GetBit(flags, CAPTURE_INITIAL_ON_START_PLAYING_BIT_POSITION);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private set => BitUtilities.SetBit(value, ref flags, CAPTURE_INITIAL_ON_START_PLAYING_BIT_POSITION);
        }

        public bool TargetIsRelative
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => BitUtilities.GetBit(flags, TARGET_IS_RELATIVE_BIT_POSITION);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private set => BitUtilities.SetBit(value, ref flags, TARGET_IS_RELATIVE_BIT_POSITION);
        }

        public InternalData(bool targetIsRelative)
        {
            this.flags = 0;
            this.initial = default;
            this.initialTweened = default;

            CaptureInitialOnStartPlaying = true;
            TargetIsRelative = targetIsRelative;
        }

        public InternalData(T initial, bool targetIsRelative)
        {
            this.flags = 0;
            this.initial = initial;
            this.initialTweened = initial;

            CaptureInitialOnStartPlaying = false;
            TargetIsRelative = targetIsRelative;
        }
    }
}
