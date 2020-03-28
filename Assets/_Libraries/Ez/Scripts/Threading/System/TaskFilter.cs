using System;

namespace Ez.Threading
{
    public class TaskFilter<T> : ITask<T>
    {
        readonly Predicate<T> _predicate;
        readonly Task<T> _task = new Task<T>();

        public TaskFilter(Predicate<T> predicate)
        {
            _predicate = predicate;
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

            else if (_predicate(message))
                return _task.PublishMessage(message);
            else
                return true;
        }
    }
}