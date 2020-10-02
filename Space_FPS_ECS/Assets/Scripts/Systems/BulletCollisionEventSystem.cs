using SpaceFpsEcs.Data;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

namespace SpaceFpsEcs.Systems
{
    [UpdateAfter(typeof(EndFramePhysicsSystem))]
    public class BulletCollisionEventSystem : JobComponentSystem
    {
        private BuildPhysicsWorld buildPhysicsWorldSystem;
        private StepPhysicsWorld stepPhysicsWorldSystem;

        protected override void OnCreate()
        {
            buildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
            stepPhysicsWorldSystem = World.GetOrCreateSystem<StepPhysicsWorld>();
        }

        struct CollisionEventImpulseJob : ICollisionEventsJob
        {
            public ComponentDataFromEntity<BulletData> BulletGroup;
            public ComponentDataFromEntity<AsteroidData> AsteroidGroup;

            public void Execute(CollisionEvent collisionEvent)
            {
                Entity entityA = collisionEvent.Entities.EntityA;
                Entity entityB = collisionEvent.Entities.EntityB;

                bool isTargetA = AsteroidGroup.Exists(entityA);
                bool isTargetB = AsteroidGroup.Exists(entityB);

                bool isBulletA = BulletGroup.Exists(entityA);
                bool isBulletB = BulletGroup.Exists(entityB);

                if (isBulletA && isTargetB)
                {
                    var asteroidComponent = AsteroidGroup[entityB];
                    var bulletComponent = BulletGroup[entityA];
                    asteroidComponent.isAlive = false;
                    bulletComponent.isAlive = false;
                    AsteroidGroup[entityB] = asteroidComponent;
                    BulletGroup[entityA] = bulletComponent;
                }
                if (isBulletB && isTargetA)
                {
                    var asteroidComponent = AsteroidGroup[entityA];
                    var bulletComponent = BulletGroup[entityB];
                    asteroidComponent.isAlive = false;
                    bulletComponent.isAlive = false;
                    AsteroidGroup[entityA] = asteroidComponent;
                    BulletGroup[entityB] = bulletComponent;
                }
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            JobHandle jobHandle = new CollisionEventImpulseJob
            {
                BulletGroup = GetComponentDataFromEntity<BulletData>(),
                AsteroidGroup = GetComponentDataFromEntity<AsteroidData>(),
            }.Schedule(stepPhysicsWorldSystem.Simulation,
                ref buildPhysicsWorldSystem.PhysicsWorld, inputDeps);

            jobHandle.Complete();
            return jobHandle;
        }
    }
}