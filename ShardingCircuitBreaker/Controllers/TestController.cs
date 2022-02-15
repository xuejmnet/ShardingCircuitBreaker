using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShardingCircuitBreaker.Entities;

namespace ShardingCircuitBreaker.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TestController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<TestController> _logger;
    private readonly ShardingDbContext _shardingDbContext;

    public TestController(ILogger<TestController> logger,ShardingDbContext shardingDbContext)
    {
        _logger = logger;
        _shardingDbContext = shardingDbContext;
    }

    public async Task<IActionResult> Test1()
    {
        var firstOrDefault = await _shardingDbContext.Set<Order>().Where(o=>o.Id=="7").OrderByDescending(o=>o.Id).FirstOrDefaultAsync();
        return Ok(firstOrDefault);
    }
    public async Task<IActionResult> Test2()
    {
        var firstOrDefault = await _shardingDbContext.Set<Order>().Where(o=>o.Id=="7").OrderByDescending(o=>o.CreateTime).FirstOrDefaultAsync();
        return Ok(firstOrDefault);
    }
}