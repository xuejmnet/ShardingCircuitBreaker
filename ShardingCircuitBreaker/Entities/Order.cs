using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/*
* @Author: xjm
* @Description:
* @Date: DATE TIME
* @Email: 326308290@qq.com
*/
namespace ShardingCircuitBreaker.Entities
{
    public class Order
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class OrderMap : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).HasMaxLength(32).IsUnicode(false);
            builder.Property(o => o.Name).HasMaxLength(255);
            builder.ToTable(nameof(Order));
        }
    }
}