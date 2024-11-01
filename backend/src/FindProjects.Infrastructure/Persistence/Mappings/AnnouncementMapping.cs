using FindProjects.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindProjects.Infrastructure.Persistence.Mappings;

public class AnnouncementMapping : IEntityTypeConfiguration<Announcement>   
{
    public void Configure(EntityTypeBuilder<Announcement> builder)
    {
        builder.ToTable("Announcements");   
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.Title)
            .HasColumnType("nvarchar(100)");
        
        builder.Property(x => x.Description)
            .HasColumnType("text")
            .HasMaxLength(2000);

        builder.HasOne(x => x.User)
            .WithMany(x => x.Announcements)
            .HasForeignKey(x => x.UserId);

        builder.HasMany(x => x.Categories)
            .WithMany(x => x.Announcements)
            .UsingEntity<Dictionary<string, object>>(
            j => j.HasOne<Category>().WithMany().HasForeignKey("CategoryId"),
            j => j.HasOne<Announcement>().WithMany().HasForeignKey("AnnouncementId"));
        
        builder.HasMany(x => x.Skills)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                j => j.HasOne<Skill>().WithMany().HasForeignKey("SkillId"),
                j => j.HasOne<Announcement>().WithMany().HasForeignKey("AnnouncementId"));
    }
}