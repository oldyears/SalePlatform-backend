using System;
using System.Collections.Generic;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.Context;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Myorder> Myorders { get; set; }

    public virtual DbSet<OrderPick> OrderPicks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("DATA SOURCE=106.15.203.185:1521/oradb;USER ID=oldyear;PASSWORD=021223;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("OLDYEAR");

        modelBuilder.Entity<Myorder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("ORDER_PK");

            entity.ToTable("MYORDER");

            entity.Property(e => e.OrderId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("ORDER_ID");
            entity.Property(e => e.CreateTime)
                .HasPrecision(6)
                .HasColumnName("CREATE_TIME");
            entity.Property(e => e.LogisticsId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("LOGISTICS_ID");
            entity.Property(e => e.Money)
                .HasColumnType("NUMBER(7,2)")
                .HasColumnName("MONEY");
            entity.Property(e => e.State)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("STATE");
            entity.Property(e => e.UserId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("USER_ID");
        });

        modelBuilder.Entity<OrderPick>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.PickId }).HasName("ORDER_PICK_PK");

            entity.ToTable("ORDER_PICK");

            entity.Property(e => e.OrderId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("ORDER_ID");
            entity.Property(e => e.PickId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("PICK_ID");
            entity.Property(e => e.Number).HasColumnType("NUMBER(38)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderPicks)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ORDER");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
