using StackExchange.Redis;

public class RedisService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public List<string> GetAllKeysAsync()
    {
        var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints()[0]);
        return server.Keys().Select(k => k.ToString()).ToList();
    }

    public async Task<string> GetKeyTypeAsync(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var keyType = await db.KeyTypeAsync(key);
        return keyType.ToString(); 
    }

    public async Task<string> GetValueAsync(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var res = await db.StringGetAsync(key);
        return res.ToString();
    }

    public async Task<Dictionary<string, string>> GetHashValuesAsync(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var hashEntries = await db.HashGetAllAsync(key);
        return hashEntries.ToDictionary(entry => entry.Name.ToString(), entry => entry.Value.ToString());
    }

    public async Task<List<string>> GetListValuesAsync(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var length = await db.ListLengthAsync(key);
        var values = await db.ListRangeAsync(key, 0, length - 1);
        return values.Select(v => v.ToString()).ToList();
    }

    public async Task<List<string>> GetSetValuesAsync(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var members = await db.SetMembersAsync(key);
        return members.Select(m => m.ToString()).ToList();
    }

    public async Task<List<SortedSetEntry>> GetSortedSetValuesAsync(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();
        return (await db.SortedSetRangeByRankWithScoresAsync(key)).ToList();
    }


    public async Task SetValueAsync(string key, string value)
    {
        var db = _connectionMultiplexer.GetDatabase();
        await db.StringSetAsync(key, value);
    }

    public async Task DeleteKeyAsync(string key)
    {
        var db = _connectionMultiplexer.GetDatabase();
        await db.KeyDeleteAsync(key);
    }
    public async Task HashSetAsync(string key, string field, string value)
    {
        var db = _connectionMultiplexer.GetDatabase();
        await db.HashSetAsync(key, field, value);
    }

    public async Task ListRightPushAsync(string key, string value)
    {
        var db = _connectionMultiplexer.GetDatabase();
        await db.ListRightPushAsync(key, value);
    }

    public async Task SetAddAsync(string key, string member)
    {
        var db = _connectionMultiplexer.GetDatabase();
        await db.SetAddAsync(key, member);
    }

    public async Task SortedSetAddAsync(string key, string member, double score)
    {
        var db = _connectionMultiplexer.GetDatabase();
        await db.SortedSetAddAsync(key, member, score);
    }

}
