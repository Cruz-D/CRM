using StackExchange.Redis;

namespace ClientsService.Infrastructure
{
    public class RedisCacheService
    {
        private readonly IDatabase _db;


        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _db = connectionMultiplexer.GetDatabase();
        }

        public async Task SetValueAsync(string key, string value)
        {
            await _db.StringSetAsync(key, value);
        }

        public async Task<string?> GetValueAsync(string key)
        {
            return await _db.StringGetAsync(key);
        }
    }
}
