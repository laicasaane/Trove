using Unity.Entities;

[System.Serializable]
public struct AttributesTester : IComponentData
{
    public int ChangingAttributesCount;
    public int ChangingAttributesChildDepth;
    public int UnchangingAttributesCount;
}
