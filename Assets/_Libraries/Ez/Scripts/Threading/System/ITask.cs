namespace Ez.Threading
{
    public interface ITask
    {
        bool ForwardCompletionTo(ITask task);

        void PublishCompletion(bool completed);
    }

    public interface ITask<T> : ITask
    {
        bool ForwardMessageTo(ITask<T> task);

        bool PublishMessage(T message);
    }
}