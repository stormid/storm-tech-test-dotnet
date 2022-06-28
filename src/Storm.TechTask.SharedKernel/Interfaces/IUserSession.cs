namespace Storm.TechTask.SharedKernel.Interfaces
{
    public interface IUserSession : IReadUserSession
    {
        void Set(string key, object value);
        void SetSingleton<T>(T value) where T : class;
    }
}
