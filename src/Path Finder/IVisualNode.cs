using GraphNamespace;

namespace VisualPathFinderNamespace
{
    public interface IVisualNodeInterface : INodeInterface
    {
        void MarkClosest();
        void MarkVisited();
        void MarkCandidate();
        void MarkPath();
    }
}