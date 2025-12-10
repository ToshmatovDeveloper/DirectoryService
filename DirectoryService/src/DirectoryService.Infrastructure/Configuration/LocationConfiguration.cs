using DirectoryService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Configuration;

public static class Index
{
    public const string NAME = "ix_location_name";
    
    public const string ADDRESS = "ix_location_address";
}

public class LocationConfiguration :IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("locations");
        
        builder.HasKey(l => l.Id)
            .HasName("pk_locations");
        
        builder.OwnsOne(l=>l.Name, nb=>
        {
            nb.Property(l => l.Value)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("name");
        });

        builder.OwnsOne(l => l.Address, ab =>
        {
            ab.Property(l => l.Country)
                .IsRequired()     
                .HasMaxLength(100)
                .HasColumnName("country");
            
            ab.Property(l => l.City)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("city");
            
            ab.Property(l => l.Street)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("street");
        });

        builder.OwnsOne(l => l.TimeZone, tb =>
        {
            tb.Property(l=>l.Value)
                .IsRequired()
                .HasColumnName("time_zone");
        });

        builder.Property(d=>d.IsActive)
            .HasDefaultValue(true)
            .HasColumnName("is_active");
        
        builder.Property(d=>d.CreatedAt)
            .HasColumnName("created_at");
        
        builder.Property(d=>d.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasIndex(x => x.Name).IsUnique().HasDatabaseName(Index.NAME);
        
        builder.HasIndex(x => x.Address).IsUnique().HasDatabaseName(Index.ADDRESS);
        
        
    }
}