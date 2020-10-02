using Unity.Entities;

namespace SpaceFpsEcs.Data
{
    [GenerateAuthoringComponent]
    public struct BulletData : IComponentData
    {
        public float speed;
        public bool isAlive;
    }
}