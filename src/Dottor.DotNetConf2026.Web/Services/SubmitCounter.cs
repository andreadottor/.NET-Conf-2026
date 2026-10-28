namespace Dottor.DotNetConf2026.Web.Services;

public sealed class SubmitCounter
{
    private long count;
    public long Count => Interlocked.Read(ref count);
    public void Increment() => Interlocked.Increment(ref count);
}