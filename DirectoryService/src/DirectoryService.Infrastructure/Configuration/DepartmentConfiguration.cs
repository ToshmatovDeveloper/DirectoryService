using DirectoryService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Configuration;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("departments");

        builder
            .HasKey(d => d.Id)
            .HasName("pk_departments");

        builder.OwnsOne(d => d.Name, nb =>
        {
            nb.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("name");
        });

        builder.OwnsOne(d => d.Identifier, ib =>
        {
            ib.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("identifier");
        });

        builder.OwnsOne(d => d.Path, pb =>
        {
            pb.Property(d => d.Value)
                .IsRequired();                
        });

        builder.Property(d => d.ParentId)
            .HasColumnName("parent_id");

        builder.HasOne(d => d.Parent)
            .WithMany(d => d.Children)
            .HasForeignKey(d => d.ParentId);

        builder.Property(d => d.Depth)
            .IsRequired()
            .HasColumnName("depth");

        builder.Property(d => d.IsActive)
            .HasDefaultValue(true)
            .HasColumnName("is_active");

        builder.Property(d => d.CreatedAt)
            .HasDefaultValueSql("NOW()")
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(d => d.Updated)
            .HasDefaultValueSql("NOW()")
            .IsRequired()
            .HasColumnName("updated_at");

        builder.HasMany(d => d.Positions)
            .WithOne(d => d.Department)
            .HasForeignKey(d => d.PositionId);
        
        builder.HasMany(d => d.Locations)
            .WithOne(d => d.Department)
            .HasForeignKey(d => d.LocationId);

    }
}
