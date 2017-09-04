using System;
using System.Collections.Generic;
using System.Text;
using DataAccessObject.Redis.Interfaces;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace DataAccessObject.Redis.Implements
{
    public class RedisRepository : IRedisRepository
    {
        private readonly IDatabase _database;

        public RedisRepository(IDatabase database)
        {
            _database = database;
        }

        public T Get<T>(string Key)
        {
            throw new NotImplementedException();
        }

        public bool IsSet(string Key)
        {
            throw new NotImplementedException();
        }

        public void Remove(string Key)
        {
            //_database.SetRemove(Key);
        }

        public void Set(string Key, object Data, int? CacheTime)
        {
            if (Data == null)
                return;

            var json = JsonConvert.SerializeObject(Data);

            if (CacheTime == null)
            {
                _database.StringSet(Key, json);
            }
            else {
                var expires = TimeSpan.FromMinutes(CacheTime.Value);

                _database.StringSet(Key, json, expires);
            }
        }
    }
}
