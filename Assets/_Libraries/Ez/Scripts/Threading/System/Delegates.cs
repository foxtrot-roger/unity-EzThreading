namespace Ez.Threading
{
    public delegate bool ProcessMessageCallback<T>(T message);
    public delegate void CompletionCallback(bool completed);
}
