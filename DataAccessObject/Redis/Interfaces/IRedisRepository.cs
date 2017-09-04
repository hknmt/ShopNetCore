using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessObject.Redis.Interfaces
{
    public interface IRedisRepository
    {
        void Set(string Key, object Data, int? CacheTime);
        T Get<T>(string Key);
        bool IsSet(string Key);
        void Remove(string Key);
    }
}
