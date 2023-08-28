using Trove.Attributes;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using AttributeChanger = Trove.Attributes.AttributeChanger<AttributeModifier, AttributeModifierStack, AttributeGetterSetter>;
using AttributeUtilities = Trove.Attributes.AttributeUtilities<AttributeModifier, AttributeModifierStack, AttributeGetterSetter>;
using AttributeCommand = Trove.Attributes.AttributeCommand<AttributeModifier, AttributeModifierStack, AttributeGetterSetter>;

public static class NameAPI
{
    private const string UNCHANGED = "Unchanged";
    private const string CHANGED = "Changed";
    private const string STRENGTH = nameof(AttributeType.Strength);
    private const string DEXTERITY = nameof(AttributeType.Dexterity);
    private const string INTELLIGENCE = nameof(AttributeType.Intelligence);

    private static readonly FixedString64Bytes FixedUnchanged = UNCHANGED;
    private static readonly FixedString64Bytes FixedChanged = CHANGED;

    public static readonly FixedString64Bytes FixedStrength = STRENGTH;
    public static readonly FixedString64Bytes FixedDexterity = DEXTERITY;
    public static readonly FixedString64Bytes FixedIntelligence = INTELLIGENCE;

    public static FixedString64Bytes GetUnchanged(int id)
    {
        var fs = FixedUnchanged;
        fs.Append('-');
        fs.Append(id);
        return fs;
    }

    public static FixedString64Bytes GetChanged(int id)
    {
        var fs = FixedChanged;
        fs.Append('-');
        fs.Append(id);
        return fs;
    }

    public static FixedString64Bytes GetChanged(int id, AttributeType attribType, int subId)
    {
        var fs = FixedChanged;

        switch (attribType)
        {
            case AttributeType.Strength:
                fs.Append(FixedStrength);
                break;

            case AttributeType.Dexterity:
                fs.Append(FixedDexterity);
                break;

            case AttributeType.Intelligence:
                fs.Append(FixedIntelligence);
                break;
        }

        fs.Append('-');
        fs.Append(id);
        fs.Append('-');
        fs.Append(subId);
        return fs;
    }
}

[System.Serializable]
public struct ChangingAttribute : IComponentData
{
    public AttributeType AttributeType;
}

public struct AttributeDuration : IComponentData
{
    public float value;
}

public struct AttributeElapsedTime : IComponentData
{
    public float value;
}

[BurstCompile]
public partial struct AttributesTesterSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var singleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();

        var job = new CreateJob {
            ecb = singleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
        };

        state.Dependency = job.ScheduleParallelByRef(state.Dependency);
    }

    [BurstCompile]
    private partial struct CreateJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        
        private void Execute(in AttributesTester test, in Entity entity, [ChunkIndexInQuery] int indexInQuery)
        {
            AttributeCommandElement.CreateAttributeCommandsEntity(
                  ecb
                , indexInQuery
                , out DynamicBuffer<AttributeCommand> commands
            );

            // Entities with changing attributes
            for (int i = 0; i < test.ChangingAttributesCount; i++)
            {
                if (i % 5 == 0)
                {
                    var newAttributeOwner = CreateAttributesOwner(ecb, indexInQuery, default, NameAPI.GetChanged(i));

                    Create(
                          ecb
                        , indexInQuery
                        , ref commands
                        , newAttributeOwner
                        , test.ChangingAttributesChildDepth
                        , i
                        , AttributeType.Strength
                    );

                    Create(
                          ecb
                        , indexInQuery
                        , ref commands
                        , newAttributeOwner
                        , test.ChangingAttributesChildDepth
                        , i
                        , AttributeType.Dexterity
                    );

                    Create(
                          ecb
                        , indexInQuery
                        , ref commands
                        , newAttributeOwner
                        , test.ChangingAttributesChildDepth
                        , i
                        , AttributeType.Intelligence
                    );
                }
                else
                {
                    var attributeType = (AttributeType)((i % 3) + 1);
                    var newAttributeOwner = CreateAttributesOwner(ecb, indexInQuery, attributeType, NameAPI.GetChanged(i));

                    Create(
                          ecb
                        , indexInQuery
                        , ref commands
                        , newAttributeOwner
                        , test.ChangingAttributesChildDepth
                        , i
                        , attributeType
                    );
                }
            }

            ecb.DestroyEntity(indexInQuery, entity);
        }

        private static void Create(
              EntityCommandBuffer.ParallelWriter ecb
            , int sortKey
            , ref DynamicBuffer<AttributeCommand> commands
            , Entity parent
            , int childDepth
            , int i
            , AttributeType attributeType
        )
        {
            for (int c = 0; c < childDepth; c++)
            {
                var last = c == childDepth - 1;
                var child = CreateAttributesOwner(ecb, sortKey, last ? attributeType : default, NameAPI.GetChanged(i, attributeType, c));

                if (last == false)
                {
                    ecb.AddComponent(sortKey, child, new AttributeDuration {
                        value = 2f
                    });

                    ecb.AddComponent<AttributeElapsedTime>(sortKey, child);
                }

                commands.Add(AttributeCommand.Create_AddModifier(
                      new AttributeReference(parent, (int)attributeType)
                    , AttributeModifier.Create_AddFromAttribute(new AttributeReference(child, (int)attributeType))
                ));

                parent = child;
            }
        }

        private static Entity CreateAttributesOwner(
              EntityCommandBuffer.ParallelWriter ecb
            , int sortKey
            , AttributeType changingAttr
            , in FixedString64Bytes name
        )
        {
            var entity = ecb.CreateEntity(sortKey);

            AttributeUtilities.MakeAttributeOwner(ecb, sortKey, entity);

            ecb.SetName(sortKey, entity, name);

            ecb.AddComponent(sortKey, entity, new Strength {
                Values = new AttributeValues(10f),
            });

            ecb.AddComponent(sortKey, entity, new Dexterity {
                Values = new AttributeValues(10f),
            });

            ecb.AddComponent(sortKey, entity, new Intelligence {
                Values = new AttributeValues(10f),
            });

            if (changingAttr == AttributeType.Strength)
            {
                ecb.AddComponent(sortKey, entity, new ChangingAttribute {
                    AttributeType = AttributeType.Strength,
                });
            }

            if (changingAttr == AttributeType.Dexterity)
            {
                ecb.AddComponent(sortKey, entity, new ChangingAttribute {
                    AttributeType = AttributeType.Dexterity,
                });
            }

            if (changingAttr == AttributeType.Intelligence)
            {
                ecb.AddComponent(sortKey, entity, new ChangingAttribute {
                    AttributeType = AttributeType.Intelligence,
                });
            }

            return entity;
        }
    }
}

[BurstCompile]
public partial struct ChangeAttributeSystem : ISystem
{
    private AttributeChanger _attributeChanger;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _attributeChanger = new AttributeChanger(ref state);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        _attributeChanger.UpdateData(ref state);

        var job = new ChangeJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            AttributeChanger = _attributeChanger,
        };

        state.Dependency = job.Schedule(state.Dependency);
    }

    [BurstCompile]
    private partial struct ChangeJob : IJobEntity
    {
        public float DeltaTime;
        public AttributeChanger AttributeChanger;

        void Execute(Entity entity, in ChangingAttribute changingAttribute)
        {
            AttributeChanger.AddBaseValue(new AttributeReference(entity, (int)changingAttribute.AttributeType), DeltaTime);
        }
    }
}

[BurstCompile]
public partial struct AttributesDestroySystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var singleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
        
        var job = new DestroyAttributesJob {
            deltaTime = SystemAPI.Time.DeltaTime,
            ecb = singleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
        };

        state.Dependency = job.ScheduleParallelByRef(state.Dependency);
    }
}

[BurstCompile]
public partial struct DestroyAttributesJob : IJobEntity
{
    public float deltaTime;
    public EntityCommandBuffer.ParallelWriter ecb;

    private void Execute(
          in Entity entity
        , in AttributeDuration duration
        , ref AttributeElapsedTime time
        , ref DynamicBuffer<AttributeObserver> observersBuffer
        , [ChunkIndexInQuery] int indexInQuery
    )
    {
        time.value += deltaTime;

        if (time.value >= duration.value)
        {
            AttributeCommandElement.CreateAttributeCommandsEntity(ecb, indexInQuery, out DynamicBuffer<AttributeCommand> attributeCommands);
            AttributeUtilities.NotifyAttributesOwnerDestruction(entity, ref observersBuffer, ref attributeCommands);
            ecb.DestroyEntity(indexInQuery, entity);
        }
    }
}
