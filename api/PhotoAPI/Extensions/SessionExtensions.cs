using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace PhotoAPI.Extensions
{
    public static class SessionExtensions
    {
        public static T Get<T>(this ISession session, string key)
        {
            var data = session.GetString(key);
            if (data == null) return default(T);
            try
            {
                var result = JsonConvert.DeserializeObject<T>(data);
                return result;
            }
            catch
            {
                return default(T);
            }
        }

        public static void Set(this ISession session, string key, object value)
        {
            var data = JsonConvert.SerializeObject(value);
            session.SetString(key, data);
        }
    }
}
