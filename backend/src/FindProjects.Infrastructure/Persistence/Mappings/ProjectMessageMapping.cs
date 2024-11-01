using FindProjects.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindProjects.Infrastructure.Persistence.Mappings;

public class ProjectMessageMapping : IEntityTypeConfiguration<ProjectMessage>
{
    public void Configure(EntityTypeBuilder<ProjectMessage> builder)
    {
        builder.ToTable("ProjectMessages");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.ProjectId).IsRequired();
        
        builder.Property(x => x.Message)
            .HasColumnType("text")
            .HasMaxLength(3000)
            .IsRequired();
        
        builder.Property(x => x.CreatedAt)
            .HasColumnType("datetime");

        builder.Property(x => x.UpdatedAt)
            .HasColumnType("datetime");

        builder.Property(x => x.SenderId)
            .IsRequired();

        builder.HasOne(x => x.Sender)
            .WithMany(x => x.ProjectMessages)
            .HasForeignKey(x => x.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Project)
            .WithMany(x => x.ProjectMessages)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}