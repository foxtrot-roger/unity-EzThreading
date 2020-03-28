using System.Collections.Generic;
using System.Linq;

namespace Ez.Threading
{
    public class TaskManager<T>
    {
        readonly Task _task = new Task();
        readonly HashSet<ITask<T>> _alive = new HashSet<ITask<T>>();

        public bool IsCompleted
        { get { return _task.IsCompleted; } }

        public void PublishCompletion(bool completed)
        {
            _alive.Clear();
            _task.PublishCompletion(completed);
        }

        public bool PublishMessage(T message)
        {
            if (IsCompleted)
                return false;

            else
            {
                var published = false;

                foreach (var data in _alive.ToArray())
                    if (data.PublishMessage(message))
                        published = true;
                    else
                    {
                        _alive.Remove(data);
                        data.PublishCompletion(true);
                    }

                return published;
            }
        }

        public ITask<T> CreateTask()
        {
            var task = new Task<T>();

            if (_task.ForwardCompletionTo(task))
                _alive.Add(task);

            return task;
        }
    }
}