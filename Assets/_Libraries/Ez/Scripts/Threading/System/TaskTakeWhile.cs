using System;

namespace Ez.Threading
{
    public class TaskTakeWhile<T> : ITask<T>
    {
        readonly Task<T> _task = new Task<T>();

        readonly Predicate<T> _predicate;

        public TaskTakeWhile(Predicate<T> predicate)
        {
            _predicate = predicate;
        }

        public bool IsCompleted
        { get { return _task.IsCompleted; } }

        public bool ForwardMessageTo(ITask<T> task)
        {
            return _task.ForwardMessageTo(task);
        }
        public bool ForwardCompletionTo(ITask data)
        {
            return _task.ForwardCompletionTo(data);
        }

        public void PublishCompletion(bool completed)
        {
            _task.PublishCompletion(completed);
        }

        public bool PublishMessage(T message)
        {
            if (IsCompleted)
                return false;

            if (_predicate(message))
                return _task.PublishMessage(message);

            else
                return false;
        }
    }
}