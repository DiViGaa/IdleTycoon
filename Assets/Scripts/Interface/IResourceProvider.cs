using Player;

namespace Interface
{
    public interface IResourceProvider
    {
        bool TryTakeResource(ResourceType type, float amount);
        float GetAmount(ResourceType type);
    }
}