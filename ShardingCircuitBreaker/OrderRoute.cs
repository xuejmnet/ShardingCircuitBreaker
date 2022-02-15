using System;
using ShardingCircuitBreaker.Entities;
using ShardingCore.Core.EntityMetadatas;
using ShardingCore.Sharding.EntityQueryConfigurations;
using ShardingCore.VirtualRoutes.Months;

/*
* @Author: xjm
* @Description:
* @Date: DATE TIME
* @Email: 326308290@qq.com
*/
namespace ShardingCircuitBreaker
{
    public class OrderRoute:AbstractSimpleShardingMonthKeyDateTimeVirtualTableRoute<Order>
    {
        public override void Configure(EntityMetadataTableBuilder<Order> builder)
        {
            builder.ShardingProperty(o => o.CreateTime);
        }

        public override bool AutoCreateTableByTime()
        {
            return true;
        }

        public override DateTime GetBeginTime()
        {
            return new DateTime(2021, 5, 1);
        }

        public override IEntityQueryConfiguration<Order> CreateEntityQueryConfiguration()
        {
            return new OrderQueryConfiguration();
        }
    }
}