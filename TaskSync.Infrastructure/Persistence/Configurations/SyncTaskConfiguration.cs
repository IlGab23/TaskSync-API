using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskSync.Domain.Entities;

namespace TaskSync.Infrastructure.Persistence.Configurations;

public class SyncTaskConfiguration : IEntityTypeConfiguration<SyncTask>
{
    public void Configure(EntityTypeBuilder<SyncTask> builder)
    {
        builder.ToTable("SyncTasks");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Description)
            .IsRequired();

        builder.ComplexProperty(s => s.DueDateUtc, dueDateBuilder =>
        {
            dueDateBuilder.Property(d => d.Value)
            .IsRequired()
            .HasColumnName("DueDateUtc");
        });

        builder.Property(s => s.IsCompleted)
            .IsRequired();

        builder.Property(s => s.UserId)
            .IsRequired();

        builder.HasOne(s => s.User)
            .WithMany(u => u.SyncTasks)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(s => s.CreatedAtUtc)
            .IsRequired();
    }
}