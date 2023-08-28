using Trove.Attributes;
using Unity.Burst;
using Unity.Entities;
using AttributeChanger = Trove.Attributes.AttributeChanger<AttributeModifier, AttributeModifierStack, AttributeGetterSetter>;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
[UpdateAfter(typeof(EndInitializationEntityCommandBufferSystem))]
public partial struct ProcessAttributeChangerCommandsSystem : ISystem
{
    private AttributeChanger _attributeChanger;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _attributeChanger = new AttributeChanger(ref state);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        _attributeChanger.UpdateData(ref state);

        state.Dependency = new AttributeChangerCommandsJob
        {
            ECB = SystemAPI.GetSingletonRW<BeginSimulationEntityCommandBufferSystem.Singleton>().ValueRW.CreateCommandBuffer(state.WorldUnmanaged),
            AttributeChanger = _attributeChanger,
            modifierNotificationsLookup = SystemAPI.GetBufferLookup<ModifierReferenceNotification>(false),
        }.Schedule(state.Dependency);
    }

    [BurstCompile]
    public partial struct AttributeChangerCommandsJob : IJobEntity
    {
        public EntityCommandBuffer ECB;
        public AttributeChanger AttributeChanger;
        public BufferLookup<ModifierReferenceNotification> modifierNotificationsLookup;

        void Execute(Entity entity, ref AttributeCommandsProcessing commandProcessing, ref DynamicBuffer<AttributeCommandElement> changerCommands)
        {
            if (commandProcessing.WasProcessed == 0)
            {
                for (int i = 0; i < changerCommands.Length; i++)
                {
                    changerCommands[i].Command.Process(ref AttributeChanger, ref modifierNotificationsLookup);
                }
                commandProcessing.WasProcessed = 1;
                ECB.DestroyEntity(entity);
            }
        }
    }
}