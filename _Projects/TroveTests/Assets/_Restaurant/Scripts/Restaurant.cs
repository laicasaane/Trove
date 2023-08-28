using Unity.Entities;
using System;

[Serializable]
public struct Restaurant : IComponentData
{
    public float NewCustomersSpeed;
    public int CleaningSuppliesCount;

    public float ReferenceTimeToTakeCustomerOrder;
    public float ReferenceTimeToCookOrder;
}

[Serializable]
public struct RestaurantState : IComponentData
{
    public int PendingOrdersCount;
    public int CustomersInLine;
    public float KitchenDirtiness;
    public int AvailableCleaningSupplies;
    public int CompletedOrders;

    public float CustomersCounter;
}