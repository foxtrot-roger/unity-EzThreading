using System;

namespace Ez.Threading
{
    public class TaskSkip<T> : ITask<T>
    {
        bool _skip = true;
        readonly Predicate<T> _predicate;
        readonly Task<T> _task = new Task<T>();

        public TaskSkip(Predicate<T> predicate)
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

            else if (!_skip)
                return _task.PublishMessage(message);

            else if (!_predicate(message))
                _skip = false;

            return true;
        }
    }
}