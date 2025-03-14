using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class SoldierAuthoring : MonoBehaviour
{
    
    public float VisionRange;
    public float AttackRange;
    public float MovementSpeed;

    class Baker : Baker<SoldierAuthoring>
    {
        public override void Bake(SoldierAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new SoldierAI
            {
                VisionRange = authoring.VisionRange,
                AttackRange = authoring.AttackRange,
                MovementSpeed = authoring.MovementSpeed
            });
            AddComponent(entity, new SoldierPathfinding());
            AddComponent(entity, new Health { Value = 100 });
        }
    }
    
}

public struct SoldierAI : IComponentData
{
    public enum State { Patrolling, Chasing, Attacking, TakingCover }
    public State CurrentState;
    public float AttackRange;
    public float VisionRange;
    public float MovementSpeed;
    public Entity TargetEntity;
}

public struct SoldierPathfinding : IComponentData
{
    public float3 TargetPosition;
    public NativeList<float3> PathWaypoints;
    public int CurrentWaypointIndex;
}

public struct Health : IComponentData
{
    public float Value;
}