using DirectoryService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Configuration;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("locations");

        builder
            .HasKey(l => l.Id)
            .HasName("pk_locations");

        builder.OwnsOne(l => l.Name, lb =>
        {
            lb.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("name");
        });

        builder.OwnsOne(l => l.Address, lb =>
        {
            lb.Property(l => l.Country)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("country");

            lb.Property(l => l.City)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("city");

            lb.Property(l => l.Street)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("street");
        });

        builder.OwnsOne(l => l.TimeZone, lb =>
        {
            lb.Property(l => l.Value)
                .IsRequired()
                .HasColumnName("time_zone");
        });

        builder.Property(l => l.IsActive)
            .HasDefaultValue(true)
            .HasColumnName("is_active");

        builder.Property(l => l.CreatedAt)
            .HasDefaultValueSql("NOW()")
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(l => l.UpdatedAt)
            .HasDefaultValueSql("NOW()")
            .IsRequired()
            .HasColumnName("updated_at");

        

    }
}
