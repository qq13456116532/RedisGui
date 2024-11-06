using StackExchange.Redis;

public class RedisCommandService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisCommandService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<string> ExecuteCommandAsync(string command)
    {
        var db = _connectionMultiplexer.GetDatabase();
        try
        {
            // 将命令字符串拆分为命令名称和参数
            var commandParts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (commandParts.Length == 0)
                return "Error: Command cannot be empty";
            var commandName = commandParts[0];
            var commandArgs = commandParts.Skip(1).ToArray();

            // 使用 ExecuteAsync 方法将完整命令字符串传递给 Redis
            RedisResult result = await db.ExecuteAsync(commandName, commandArgs);
            var ToArray=((string?[]?)result);
            string strRes = string.Join(", ", ToArray!);
            return strRes; // 将 Redis 的原始响应返回为字符串
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}"; // 错误信息返回
        }
    }
}



