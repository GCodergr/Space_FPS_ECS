using Unity.Entities;

namespace SpaceFpsEcs.Data
{
    [GenerateAuthoringComponent]
    public struct AsteroidData : IComponentData
    {
        public bool isAlive;
    }
}