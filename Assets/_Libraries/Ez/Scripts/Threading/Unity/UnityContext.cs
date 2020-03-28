namespace Ez.Threading
{
    public static class UnityContext
    {
        public static TaskManager<float> Update = new TaskManager<float>();
        public static TaskManager<float> FixedUpdate = new TaskManager<float>();
    }
}