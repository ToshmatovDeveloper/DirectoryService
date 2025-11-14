using DirectoryService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Configuration;

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable("positions");
        
        builder.HasKey(p => p.Id)
            .HasName("pk_positions");
        
        builder.OwnsOne(p => p.Name, pb =>
        {
            pb.Property(d=>d.Value)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("name");
        });
        
        builder.Property(p=>p.Description)
            .IsRequired()
            .HasColumnName("description");
        
        builder.Property(d=>d.IsActive)
            .HasDefaultValue(true)
            .HasColumnName("is_active");
        
        builder.Property(d=>d.CreatedAt)
            .HasColumnName("created_at");
        
        builder.Property(d=>d.UpdatedAt)
            .HasColumnName("updated_at");
    }
}