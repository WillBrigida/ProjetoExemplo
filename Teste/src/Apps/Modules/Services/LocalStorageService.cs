using Core.Modules.Services;

namespace Apps.Modules.Services
{
    public class LocalStorageService : ILocalStorageService
    {
        public object Get(string key, object defaltValue)
        {
            if (defaltValue is string)
                return Preferences.Get(key, (string)defaltValue);

            else if (defaltValue is bool)
                return Preferences.Get(key, (bool)defaltValue);

            else if (defaltValue is DateTime)
                return Preferences.Get(key, (DateTime)defaltValue);

            else if (defaltValue is float)
                return Preferences.Get(key, (float)defaltValue);

            else if (defaltValue is double)
                return Preferences.Get(key, (double)defaltValue);

            else if (defaltValue is int)
                return Preferences.Get(key, (int)defaltValue);

            else if (defaltValue is long)
                return Preferences.Get(key, (long)defaltValue);

            return null;
        }

        public void Set(string key, object value)
        {
            if (value is string)
                Preferences.Set(key, (string)value);

            else if (value is bool)
                Preferences.Set(key, (bool)value);

            else if (value is DateTime)
                Preferences.Set(key, (DateTime)value);

            else if (value is float)
                Preferences.Set(key, (float)value);

            else if (value is double)
                Preferences.Set(key, (double)value);

            else if (value is int)
                Preferences.Set(key, (int)value);

            else if (value is long)
                Preferences.Set(key, (long)value);
        }

        public void Remove(string key) => Preferences.Remove(key);
    }
}
