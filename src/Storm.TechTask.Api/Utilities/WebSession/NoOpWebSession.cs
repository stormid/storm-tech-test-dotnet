using Storm.TechTask.SharedKernel.Interfaces;

namespace Storm.TechTask.Api.Utilities.WebSession
{
    public class NoOpWebSession : IUserSession
    {
        public void Set(string key, object value)
        {
        }

        public T? Get<T>(string key) where T : class
        {
            return default(T);
        }

        public void SetSingleton<T>(T value) where T : class
        {
        }

        public T? GetSingleton<T>() where T : class
        {
            return default(T);
        }
    }
}
