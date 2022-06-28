namespace Storm.TechTask.SharedKernel.Interfaces
{
    public interface IReadUserSession
    {
        T? Get<T>(string key) where T : class;
        T? GetSingleton<T>() where T : class;
    }
}
