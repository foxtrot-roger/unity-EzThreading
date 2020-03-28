using System;

namespace Ez.Threading
{
    public static class TaskExtension
    {
        private static void IgnoreCompletion(bool completed)
        {
        }
        private static bool WaitForCompletion<T>(T message)
        {
            return false;
        }


        public static ITask<T> ProcessWith<T>(this ITask<T> self, Action<T> processMessageCallback)
        {
            return self.ProcessWith(messasge =>
            {
                processMessageCallback(messasge);
                return true;
            });
        }
        public static ITask<T> ProcessWith<T>(this ITask<T> self, ProcessMessageCallback<T> processMessageCallback)
        {
            var data = new DelegateTask<T>(processMessageCallback, IgnoreCompletion);

            self.ForwardMessageTo(data);

            return self;
        }

        public static ITask<T> ContinueWith<T>(this ITask<T> self, CompletionCallback completionCallback)
        {
            var data = new DelegateTask<T>(WaitForCompletion, completionCallback);

            self.ForwardMessageTo(data);

            return self;
        }
        public static ITask ContinueWith(this ITask self, CompletionCallback completionCallback)
        {
            var data = new DelegateTask(completionCallback);

            self.ForwardCompletionTo(data);

            return self;
        }

        public static ITask<T> Where<T>(this ITask<T> self, Predicate<T> predicate)
        {
            var result = new TaskFilter<T>(predicate);

            self.ForwardMessageTo(result);

            return result;
        }
        public static ITask<TOut> Select<TIn, TOut>(this ITask<TIn> self, Converter<TIn, TOut> converter)
        {
            var result = new Task<TOut>();

            self.ForwardMessageTo(new DelegateTask<TIn>(
                message => result.PublishMessage(converter(message)),
                result.PublishCompletion));

            return result;
        }

        public static ITask<T> Skip<T>(this ITask<T> self, int count)
        {
            var i = 1;
            return self.SkipWhile(message => i++ < count);
        }
        public static ITask<T> SkipWhile<T>(this ITask<T> self, Predicate<T> predicate)
        {
            var result = new TaskSkip<T>(predicate);

            self.ForwardMessageTo(result);

            return result;
        }

        public static ITask<T> Take<T>(this ITask<T> self, int count)
        {
            var result = new TaskTakeCount<T>(count);

            self.ForwardMessageTo(result);

            return result;
        }
        public static ITask<T> TakeWhile<T>(this ITask<T> self, Predicate<T> predicate)
        {
            var result = new TaskTakeWhile<T>(predicate);

            self.ForwardMessageTo(result);

            return result;
        }

        public static ITask<T> First<T>(this ITask<T> self)
        {
            return self.Take(1);
        }
    }
}