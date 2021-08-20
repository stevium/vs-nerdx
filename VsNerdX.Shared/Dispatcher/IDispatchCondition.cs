namespace VsNerdX.Dispatcher
{
    public interface IDispatchCondition
    {
        bool ShouldDispatch { get; }
    }
}