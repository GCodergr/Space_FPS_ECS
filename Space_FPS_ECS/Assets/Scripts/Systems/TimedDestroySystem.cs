using SpaceFpsEcs.Data;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace SpaceFpsEcs.Systems
{
    public class TimedDestroySystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            float deltaTime = Time.DeltaTime;
            Entities.WithoutBurst().WithStructuralChanges()
                .ForEach((Entity entity, ref LifetimeData lifetimeData) =>
                {
                    lifetimeData.timeLeft -= deltaTime;
                    if (lifetimeData.timeLeft <= 0f)
                        EntityManager.DestroyEntity(entity);
                })
                .Run();

            Entities.WithoutBurst().WithStructuralChanges()
                .ForEach((Entity entity, ref Translation position, ref AsteroidData asteroidData) =>
                {
                    if (!asteroidData.isAlive)
                    {
                        SpawnManager.Instance.SpawnCoins(position);

                        EntityManager.DestroyEntity(entity);
                    }
                })
                .Run();

            Entities.WithoutBurst().WithStructuralChanges()
                .ForEach((Entity entity, ref Translation position, ref BulletData bulletData) =>
                {
                    if (!bulletData.isAlive)
                    {
                        EntityManager.DestroyEntity(entity);
                    }
                })
                .Run();

            return inputDeps;
        }
    }
}