using System.Collections.Generic;
using System.Linq;

namespace Ez.Threading
{
    public class Task : ITask
    {
        readonly HashSet<ITask> _waitingCompletion = new HashSet<ITask>();

        bool _completionState;

        public bool IsCompleted { get; private set; }

        public bool ForwardCompletionTo(ITask task)
        {
            if (IsCompleted)
            {
                task.PublishCompletion(_completionState);

                return false;
            }
            else
            {
                _waitingCompletion.Add(task);

                return true;
            }
        }

        public void PublishCompletion(bool completed)
        {
            if (IsCompleted)
                return;

            IsCompleted = true;
            _completionState = completed;

            foreach (var data in _waitingCompletion.ToArray())
                data.PublishCompletion(completed);

            _waitingCompletion.Clear();
        }
    }

    public class Task<T> : ITask<T>
    {
        readonly Task _task = new Task();
        readonly HashSet<ITask<T>> _alive = new HashSet<ITask<T>>();

        public bool IsCompleted
        { get { return _task.IsCompleted; } }

        public bool ForwardMessageTo(ITask<T> task)
        {
            if (_task.ForwardCompletionTo(task))
            {
                _alive.Add(task);
                return true;
            }
            else
                return false;
        }
        public bool ForwardCompletionTo(ITask task)
        {
            return _task.ForwardCompletionTo(task);
        }

        public void PublishCompletion(bool completed)
        {
            //this.Log("COMPLETE TO #" + _alive.Count + "/" + _waitingCompletion.Count + " c:" + IsCompleted + " m:" + completed + "\n" +
            //    "Alive  : " + string.Join(", ", _alive.Select(o => o.GetName()).ToArray()) + "\n" +
            //    "Waiting: " + string.Join(", ", _waitingCompletion.Select(o => o.GetName()).ToArray()) + "\n");

            //this.Log("COMPLETE TO #" + _alive.Count + " c:" + IsCompleted + " m:" + completed + "\n" +
            //    "Alive  : " + string.Join(", ", _alive.Select(o => o.GetName()).ToArray()) + "\n");

            _alive.Clear();
            _task.PublishCompletion(completed);
        }

        public bool PublishMessage(T message)
        {
            //this.Log("PUBLISH TO #" + _alive.Count + "/" + _waitingCompletion.Count + " c:" + IsCompleted + " m:" + message + "\n" +
            //    "Alive  : " + string.Join(", ", _alive.Select(o => o.GetName()).ToArray()) + "\n" +
            //    "Waiting: " + string.Join(", ", _waitingCompletion.Select(o => o.GetName()).ToArray()) + "\n");

            if (IsCompleted)
                return false;

            else
            {
                var published = false;

                foreach (var data in _alive.ToArray())
                    if (data.PublishMessage(message))
                        published = true;
                    else
                        _alive.Remove(data);

                return published;
            }
        }
    }
}