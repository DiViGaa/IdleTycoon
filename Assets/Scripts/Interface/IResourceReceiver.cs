using Player;

namespace Interface
{
    public interface IResourceReceiver
    {
        void AddResource(ResourceType type, float amount);
    }
}