using DirectoryService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryService.Infrastructure.Configuration
{
    public class DepartmentLocationConfiguration : IEntityTypeConfiguration<DepartmentLocation>
    {
        public void Configure(EntityTypeBuilder<DepartmentLocation> builder)
        {
            builder.ToTable("department_location");

            builder.HasKey(dl => new { dl.DepartmentId, dl.LocationId });

            builder.HasOne(dl => dl.Department)
                .WithMany(dl => dl.Locations)
                .HasForeignKey(dl => dl.DepartmentId);
            
            builder.HasOne(dl => dl.Location)
                .WithMany(dl => dl.Departments)
                .HasForeignKey(dl => dl.DepartmentId);
        }
    }
}
