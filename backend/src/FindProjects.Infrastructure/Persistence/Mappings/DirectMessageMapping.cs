using FindProjects.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindProjects.Infrastructure.Persistence.Mappings;

public class DirectMessageMapping : IEntityTypeConfiguration<DirectMessage>
{
    public void Configure(EntityTypeBuilder<DirectMessage> builder)
    {
        builder.ToTable("DirectMessages");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.ReceiverId).IsRequired();
        
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
            .WithMany(x => x.SentMessages)
            .HasForeignKey(x => x.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Receiver)
            .WithMany(x => x.ReceivedMessages)
            .HasForeignKey(x => x.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}