using Unity.Entities;
using System;
using Trove.Tweens;

[Serializable]
public struct LocalPositionTargetTween : IComponentData
{
    public TweenTimer Timer;
    public TweenerFloat3 Tweener;
    public Entity Target;

    public LocalPositionTargetTween(Entity target, TweenerFloat3 tweener, TweenTimer timer)
    {
        Target = target;
        Timer = timer;
        Tweener = tweener;
    }
}

[Serializable]
public struct LocalPositionBufferTween : IBufferElementData
{
    public enum Type
    {
        X,
        Y,
        Z,
    }

    public TweenTimer Timer;
    public TweenerFloat Tweener;
    public Type PosType;

    public LocalPositionBufferTween(TweenerFloat tweener, TweenTimer timer, Type posType)
    {
        Timer = timer;
        Tweener = tweener;
        PosType = posType;
    }
}


