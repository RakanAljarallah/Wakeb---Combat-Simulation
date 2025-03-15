using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class TargetsAuthering : MonoBehaviour
{
    public class TargetAuthoring : MonoBehaviour
    {
        public float WanderRadius;
        public float RespawnTime;
        public GameObject HealthBarPrefab; 

        class Baker : Baker<TargetAuthoring>
        {
            public override void Bake(TargetAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new TargetAI
                {
                    WanderRadius = authoring.WanderRadius,
                    SpawnPosition = authoring.transform.position
                });
                AddComponent(entity, new Health { Value = 50 });
                AddComponent(entity, new Respawn
                {
                    Timer = 0,
                    RespawnTime = authoring.RespawnTime,
                    SpawnPosition = authoring.transform.position
                });
                
            }
        }
    }
}
public struct Respawn : IComponentData
{
    public float Timer;
    public float RespawnTime;
    public float3 SpawnPosition;
}
public struct TargetAI : IComponentData
{
    public float MovementSpeed;
    public float3 SpawnPosition;
    public float WanderRadius;
}