using FindProjects.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindProjects.Infrastructure.Persistence.Mappings;

public class ProjectMapping : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");

        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Title)
            .HasColumnType("nvarchar(255)")
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnType("text")
            .HasMaxLength(3000);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>() 
            .IsRequired();

        builder.Property(x => x.DeadLine)
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(x => x.HasBudget)
            .HasColumnType("bit")
            .IsRequired();

        builder.Property(x => x.Budget)
            .HasColumnType("decimal(18,2)"); 

        builder.Property(x => x.MaxContributors)
            .HasColumnType("int")
            .HasDefaultValue(999);

        builder.Property(x => x.CreatedAt)
            .HasColumnType("datetime");

        builder.Property(x => x.ProjectLink)
            .HasColumnType("nvarchar(500)");

        builder.HasOne(x => x.User)
            .WithMany(u => u.Projects)
            .HasForeignKey(x => x.UserId);

        builder.HasMany(x => x.Skills)
            .WithMany(s => s.Projects)
            .UsingEntity<Dictionary<string, object>>(
                "ProjectSkill",
                j => j.HasOne<Skill>().WithMany().HasForeignKey("SkillId"),
                j => j.HasOne<Project>().WithMany().HasForeignKey("ProjectId")
            );

        builder.HasMany(x => x.Categories)
            .WithMany(c => c.Projects)
            .UsingEntity<Dictionary<string, object>>(
                "ProjectCategory",
                j => j.HasOne<Category>().WithMany().HasForeignKey("CategoryId"),
                j => j.HasOne<Project>().WithMany().HasForeignKey("ProjectId")
            );
    }
}