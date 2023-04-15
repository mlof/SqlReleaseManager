namespace SqlReleaseManager.Core.Abstractions;

public interface ISqlServer : IDisposable
{
    public Task<bool> CanConnect();
}