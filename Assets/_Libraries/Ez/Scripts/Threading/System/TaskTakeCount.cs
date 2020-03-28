namespace Ez.Threading
{
    public class TaskTakeCount<T> : ITask<T>
    {
        readonly Task<T> _task = new Task<T>();

        int _remaining;

        public TaskTakeCount(int count)
        {
            _remaining = count;
        }

        public bool IsCompleted
        { get { return _task.IsCompleted; } }

        public bool ForwardMessageTo(ITask<T> task)
        {
            return _task.ForwardMessageTo(task);
        }
        public bool ForwardCompletionTo(ITask task)
        {
            return _task.ForwardCompletionTo(task);
        }

        public void PublishCompletion(bool completed)
        {
            _task.PublishCompletion(completed);
        }

        public bool PublishMessage(T message)
        {
            if (IsCompleted)
                return false;

            if (0 < _remaining--)
                return _task.PublishMessage(message);

            else
                return false;
        }
    }
}