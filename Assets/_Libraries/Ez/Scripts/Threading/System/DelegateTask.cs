namespace Ez.Threading
{
    public class DelegateTask : ITask
    {
        readonly CompletionCallback _completionCallback;

        public bool IsCompleted
        { get; private set; }

        public DelegateTask(CompletionCallback completionCallback)
        {
            _completionCallback = completionCallback;
        }

        public bool ForwardCompletionTo(ITask task)
        {
            return false;
        }

        public void PublishCompletion(bool completed)
        {
            if (IsCompleted)
                return;

            IsCompleted = true;

            if (_completionCallback != null)
                _completionCallback(completed);
        }
    }

    public class DelegateTask<T> : ITask<T>
    {
        readonly DelegateTask _task;
        readonly ProcessMessageCallback<T> _processMessageCallback;

        public bool IsCompleted
        { get { return _task.IsCompleted; } }

        public DelegateTask(
            ProcessMessageCallback<T> processMessageCallback,
            CompletionCallback completionCallback)
        {
            _processMessageCallback = processMessageCallback;
            _task = new DelegateTask(completionCallback);
        }

        public bool ForwardMessageTo(ITask<T> task)
        {
            return false;
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

            else if (_processMessageCallback != null)
                return _processMessageCallback(message);

            else
                return false;
        }
    }
}