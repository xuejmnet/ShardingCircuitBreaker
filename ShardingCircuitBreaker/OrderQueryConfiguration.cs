using System;
using ShardingCircuitBreaker.Entities;
using ShardingCore.Sharding.EntityQueryConfigurations;

/*
* @Author: xjm
* @Description:
* @Date: DATE TIME
* @Email: 326308290@qq.com
*/
namespace ShardingCircuitBreaker
{
    public class OrderQueryConfiguration:IEntityQueryConfiguration<Order>
    {
        public void Configure(EntityQueryBuilder<Order> builder)
        {
            //202105，202106...是默认的顺序,false表示使用反向排序,就是如果存在分片那么分片的tail将进行反向排序202202,202201,202112,202111....
            builder.ShardingTailComparer(Comparer<string>.Default, false);
            //order by createTime asc的顺序和分片ShardingTailComparer一样那么就用true
            //但是目前ShardingTailComparer是倒序所以order by createTime asc需要和他一样必须要是倒序,倒序就是false
            builder.AddOrder(o => o.CreateTime,false);
            //配置当不存在Order的时候如果我是FirstOrDefault那么将才用和ShardingTailComparer相反的排序执行因为是false
            builder.AddDefaultSequenceQueryTrip(true, CircuitBreakerMethodNameEnum.FirstOrDefault);
            //内部配置单表查询的FirstOrDefault connections limit限制为1
            builder.AddConnectionsLimit(1, LimitMethodNameEnum.FirstOrDefault);
        }
    }
}