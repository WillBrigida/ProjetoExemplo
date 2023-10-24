namespace Core.Modules.Services
{
    public interface ILocalStorageService
    {
        object Get(string key, object defaltValue);
        void Set(string key, object value);
        void Remove(string key);
    }
}
