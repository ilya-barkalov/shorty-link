using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SL.Domain.Entities;

namespace SL.Infrastructure.Persistence.Configurations;

public class LinkVisitConfiguration : IEntityTypeConfiguration<LinkVisit>
{
    public void Configure(EntityTypeBuilder<LinkVisit> builder)
    {
        builder.ToTable("LinkVisit");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.LinkId)
            .HasMaxLength(300)
            .IsRequired();
        
        builder.Property(e => e.CreatedDate)
            .IsRequired();

        builder.HasOne(e => e.Link)
            .WithMany(e => e.Visits)
            .HasForeignKey(e => e.LinkId)
            .IsRequired();
    }
}