using DirectoryService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryService.Infrastructure.Configuration;

public class DepartmentPositionConfiguration : IEntityTypeConfiguration<DepartmentPosition>
{
    public void Configure(EntityTypeBuilder<DepartmentPosition> builder)
    {
        builder.ToTable("department_position_configuration");
        
        builder.HasKey(dp => new { dp.DepartmentId, dp.PositionId });
        
        builder.HasOne(dp => dp.Department)
            .WithMany(dp => dp.Positions)
            .HasForeignKey(dp => dp.DepartmentId);
        
        builder.HasOne(dp => dp.Position)
            .WithMany(dp => dp.Departments)
            .HasForeignKey(dp => dp.PositionId);
    }
}