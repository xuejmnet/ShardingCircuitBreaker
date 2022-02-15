using System;
using Microsoft.EntityFrameworkCore;
using ShardingCircuitBreaker.Entities;
using ShardingCore.Core.VirtualRoutes.TableRoutes.RouteTails.Abstractions;
using ShardingCore.Sharding;
using ShardingCore.Sharding.Abstractions;

/*
* @Author: xjm
* @Description:
* @Date: DATE TIME
* @Email: 326308290@qq.com
*/
namespace ShardingCircuitBreaker
{
    public class ShardingDbContext:AbstractShardingDbContext,IShardingTableDbContext
    {
        public ShardingDbContext(DbContextOptions<ShardingDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new OrderMap());
        }

        public IRouteTail RouteTail { get; set; }
    }
}