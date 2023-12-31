using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.Domain.Entities;

namespace SL.Infrastructure.Persistence.Configurations;

public class LinkConfiguration : IEntityTypeConfiguration<Link>
{
    public void Configure(EntityTypeBuilder<Link> builder)
    {
        builder.ToTable("Link");

        builder.HasKey(e => e.Id);
        
        builder.HasIndex(e => e.ShortUrl);

        builder.Property(e => e.OriginalUrl)
            .HasMaxLength(300)
            .IsRequired();
        
        builder.Property(e => e.ShortUrl)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(e => e.CreatedDate)
            .IsRequired();

        builder.HasMany(e => e.Visits)
            .WithOne(e => e.Link)
            .HasForeignKey(e => e.LinkId)
            .IsRequired();
    }
}