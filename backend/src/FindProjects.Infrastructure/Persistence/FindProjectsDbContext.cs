using FindProjects.Core.Entities;
using FindProjects.Infrastructure.Persistence.Mappings;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FindProjects.Infrastructure.Persistence;

public class FindProjectsDbContext : IdentityDbContext<User>
{
    public FindProjectsDbContext(DbContextOptions<FindProjectsDbContext> options) : base(options) {}

    public DbSet<Project> Projects { get; set; }
    public DbSet<Contributor> Contributors { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Announcement> Announcements { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<ProjectMessage> ProjectMessages { get; set; }
    public DbSet<DirectMessage> DirectMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new AnnouncementMapping());
        builder.ApplyConfiguration(new CategoryMapping());
        builder.ApplyConfiguration(new ContributorMapping());
        builder.ApplyConfiguration(new DirectMessageMapping());
        builder.ApplyConfiguration(new ProjectMessageMapping());
        builder.ApplyConfiguration(new ProjectMapping());
        builder.ApplyConfiguration(new SkillMapping());
    }
}  