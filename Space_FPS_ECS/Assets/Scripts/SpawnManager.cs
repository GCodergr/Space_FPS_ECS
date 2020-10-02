using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace SpaceFpsEcs
{
    public class SpawnManager : MonoBehaviour
    {
        #region Fields and Properties

        [SerializeField] private GameObject asteroid1Prefab = default;

        [SerializeField] private GameObject bulletPrefab = default;

        [SerializeField] private GameObject coinPrefab = default;

        [Space] [SerializeField] private int asteroidCount = 500;

        private Entity bulletEntity;

        private static readonly int coinCount = 5;
        private Entity coinEntity;

        private BlobAssetStore blobAssetStore;

        private EntityManager entityManager;

        private PlayerManager playerManager;

        private static SpawnManager instance;

        public static SpawnManager Instance => instance;

        #endregion

        private void Awake()
        {
            if (instance is null)
            {
                instance = FindObjectOfType<SpawnManager>();
            }
        }

        private void Start()
        {
            blobAssetStore = new BlobAssetStore();
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var settings =
                GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
            Entity asteroidEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(asteroid1Prefab, settings);
            bulletEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefab, settings);
            coinEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(coinPrefab, settings);

            playerManager = FindObjectOfType<PlayerManager>();

            SpawnAsteroids(asteroidEntity);
        }

        private void SpawnAsteroids(Entity asteroidEntity)
        {
            for (int i = 0; i < asteroidCount; i++)
            {
                Entity asteroid1Instance = entityManager.Instantiate(asteroidEntity);
                entityManager.SetName(asteroidEntity, "Asteroid");
                float x = UnityEngine.Random.Range(-50f, 50f);
                float y = UnityEngine.Random.Range(-50f, 50f);
                float z = UnityEngine.Random.Range(-50f, 50f);

                float3 position = new float3(x, y, z);
                entityManager.SetComponentData(asteroid1Instance, new Translation {Value = position});
            }
        }

        public void SpawnCoins(Translation position)
        {
            for (int i = 0; i < coinCount; i++)
            {
                float3 offset = (float3) UnityEngine.Random.insideUnitSphere * 2.0f;
                var coinInstance = entityManager.Instantiate(coinEntity);
                entityManager.SetName(coinEntity, "Coin");
                float3 randomDir = new float3(UnityEngine.Random.Range(-1, 1),
                    UnityEngine.Random.Range(-1, 1),
                    UnityEngine.Random.Range(-1, 1));
                entityManager.SetComponentData<Translation>(coinInstance,
                    new Translation {Value = position.Value + offset});
                entityManager.SetComponentData<PhysicsVelocity>(coinInstance,
                    new PhysicsVelocity {Linear = randomDir * 2});
            }

            playerManager.AddCredits(coinCount);
        }

        public void SpawnShipBullet()
        {
            var bulletInstance = entityManager.Instantiate(bulletEntity);
            entityManager.SetName(bulletEntity, "Bullet");

            var startPosition = playerManager.BulletSpawnTransform.position;
            entityManager.SetComponentData(bulletInstance, new Translation {Value = startPosition});
            entityManager.SetComponentData(bulletInstance,
                new Rotation {Value = playerManager.MyTransform.rotation});
        }

        private void OnDestroy()
        {
            blobAssetStore.Dispose();
        }
    }
}