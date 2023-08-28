using Unity.Entities;
using UnityEngine;
using AttributeUtilities = Trove.Attributes.AttributeUtilities<AttributeModifier, AttributeModifierStack, AttributeGetterSetter>;

[DisallowMultipleComponent]
public class AttributeOwnerAuthoring : MonoBehaviour
{
    class Baker : Baker<AttributeOwnerAuthoring>
    {
        public override void Bake(AttributeOwnerAuthoring authoring)
        {
            AttributeUtilities.MakeAttributeOwner(this);
        }
    }
}
