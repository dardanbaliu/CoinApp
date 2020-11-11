using CoinApp.Domain.Coins;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoinApp.Infrastructure.Coins
{
    public class CoinEntityTypeConfiguration : IEntityTypeConfiguration<Coin>
    {
        public void Configure(EntityTypeBuilder<Coin> builder)
        {
            builder.Property(e => e.Id);
            builder.Property(e => e.UserId).IsRequired();
            builder.HasOne(d => d.User)
                   .WithMany(p => p.Coins)
                   .HasForeignKey(d => d.UserId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_User_Coins");
        }
    }
}
