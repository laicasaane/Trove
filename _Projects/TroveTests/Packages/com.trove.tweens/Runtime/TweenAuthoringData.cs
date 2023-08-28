using System;

namespace Trove.Tweens
{
    [Serializable]
    public struct TweenAuthoringData
    {
        public float duration;
        public float speed;
        public bool isLoop;
        public bool isRewind;

        public static TweenAuthoringData GetDefault()
        {
            return new TweenAuthoringData {
                isLoop = false,
                isRewind = false,
                duration = 1f,
                speed = 1f,
            };
        }
    }
}
