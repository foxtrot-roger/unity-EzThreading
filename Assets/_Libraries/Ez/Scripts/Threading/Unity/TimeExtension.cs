using UnityEngine;

namespace Ez.Threading
{
    public static class TimeExtension
    {
        public static ITask<float> StartingIn(this ITask<float> self, float durationInSeconds)
        {
            return self.StartingAt(Time.time + durationInSeconds);
        }
        public static ITask<float> StartingAt(this ITask<float> self, float startInSeconds)
        {
            return self.SkipWhile(now => now < startInSeconds);
        }

        public static ITask<float> EndingIn(this ITask<float> self, float durationInSeconds)
        {
            return self.EndingAt(Time.time + durationInSeconds);
        }
        public static ITask<float> EndingAt(this ITask<float> self, float endInSeconds)
        {
            return self.TakeWhile(now => now < endInSeconds);
        }

        public static ITask<float> RelativeToStart(this ITask<float> self)
        {
            return self.RelativeToStart(Time.time);
        }
        public static ITask<float> RelativeToStart(this ITask<float> self, float startInSeconds)
        {
            return self.Select(now => now - startInSeconds);
        }

        public static ITask<float> RelativeToLast(this ITask<float> self)
        {
            return self.RelativeToLast(Time.time);
        }
        public static ITask<float> RelativeToLast(this ITask<float> self, float startInSeconds)
        {
            return self.Select(now =>
            {
                var delta = now - startInSeconds;
                startInSeconds = now;

                return delta;
            });
        }

        public static ITask<float> AtFequency(this ITask<float> self, float countPerDuration, float durationInSeconds = 1)
        {
            return self.AtInterval(durationInSeconds / countPerDuration);
        }

        public static ITask<float> AtInterval(this ITask<float> self, float intervalInSecond)
        {
            return self.AtInterval(Time.time, intervalInSecond);
        }
        public static ITask<float> AtInterval(this ITask<float> self, float start, float intervalInSecond)
        {
            var next = start + intervalInSecond;
            return self
                .Where(now =>
                {
                    if (next <= now)
                    {
                        next = next + intervalInSecond;
                        return true;
                    }
                    else
                        return false;
                });
        }
    }
}