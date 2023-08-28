using Trove.Attributes;
using System;
using Unity.Entities;

[Serializable]
public struct Strength : IComponentData
{
    public AttributeValues Values;
}

[Serializable]
public struct Dexterity : IComponentData
{
    public AttributeValues Values;
}

[Serializable]
public struct Intelligence : IComponentData
{
    public AttributeValues Values;
}