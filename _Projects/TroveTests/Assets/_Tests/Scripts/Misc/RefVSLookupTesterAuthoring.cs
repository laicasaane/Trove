using Unity.Entities;
using UnityEngine;

public class RefVSLookupTesterAuthoring : MonoBehaviour
{
    public int HowMany = 10000;

    class Baker : Baker<RefVSLookupTesterAuthoring>
    {
        public override void Bake(RefVSLookupTesterAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.None), new RefVSLookupTester
            {
                HowMany = authoring.HowMany,
            });
        }
    }
}