using Unity.Entities;
using UnityEngine;

public class RestaurantAuthoring : MonoBehaviour
{
    public Restaurant RestaurantParameters;

    class Baker : Baker<RestaurantAuthoring>
    {
        public override void Bake(RestaurantAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.None), authoring.RestaurantParameters);
            AddComponent(GetEntity(TransformUsageFlags.None), new RestaurantState
            {
                PendingOrdersCount = 0,
                CustomersInLine = 0,
                KitchenDirtiness = 0f,
                AvailableCleaningSupplies = authoring.RestaurantParameters.CleaningSuppliesCount,
            });
        }
    }
}