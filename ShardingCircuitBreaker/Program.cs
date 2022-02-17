using Microsoft.EntityFrameworkCore;
using ShardingCircuitBreaker;
using ShardingCircuitBreaker.Entities;
using ShardingCore;
using ShardingCore.Bootstrapers;
using ShardingCore.TableExists;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ILoggerFactory efLogger = LoggerFactory.Create(builder =>
{
    builder.AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information).AddConsole();
});
builder.Services.AddControllers();
builder.Services.AddShardingDbContext<ShardingDbContext>()
    .AddEntityConfig(op =>
    {
        op.CreateShardingTableOnStart = true;
        op.EnsureCreatedWithOutShardingTable = true;
        op.AddShardingTableRoute<OrderRoute>();
        op.UseShardingQuery((conStr, b) =>
        {
            b.UseMySql(conStr, new MySqlServerVersion(new Version())).UseLoggerFactory(efLogger);
        });
        op.UseShardingTransaction((conn, b) =>
        {
            b.UseMySql(conn, new MySqlServerVersion(new Version())).UseLoggerFactory(efLogger);
        });
    }).AddConfig(op =>
    {
        op.ConfigId = "c1";
        op.AddDefaultDataSource("ds0", "server=127.0.0.1;port=3306;database=db1;userid=root;password=L6yBtV6qNENrwBy7;");
        op.ReplaceTableEnsureManager(sp=>new MySqlTableEnsureManager<ShardingDbContext>());
    }).EnsureConfig();
var app = builder.Build();

app.Services.GetRequiredService<IShardingBootstrapper>().Start();
using (var scope=app.Services.CreateScope())
{
    var shardingDbContext = scope.ServiceProvider.GetRequiredService<ShardingDbContext>();
    if (!shardingDbContext.Set<Order>().Any())
    {
        var begin = new DateTime(2021, 5, 2);
        List<Order> orders = new List<Order>(8);
        for (int i = 0; i < 8; i++)
        {
            orders.Add(new Order()
            {
                Id = i.ToString(),
                Name = $"{begin:yyyy-MM-dd HH:mm:ss}",
                CreateTime = begin
            });
            begin = begin.AddMonths(1);
        }
        shardingDbContext.AddRange(orders);
        shardingDbContext.SaveChanges();
    }
}
app.UseAuthorization();
app.MapControllers();
app.Run();