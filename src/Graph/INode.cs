namespace GraphNamespace
{
    public interface INodeInterface
    {
        bool IsAvailable { get; set; }
        bool CanGoTo(INodeInterface node);
    }
}