using Unity.Entities;

namespace SpaceFpsEcs.Data
{
    [GenerateAuthoringComponent]
    public struct LifetimeData : IComponentData
    {
        public float timeLeft;
    }
}