using LibraryManagement.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Persistence.Configurations
{
    public class ErrorLogConfig : IEntityTypeConfiguration<ErrorLog>
    {
        public void Configure(EntityTypeBuilder<ErrorLog> builder)
        {
            builder.ToTable("ErrorLogs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Message)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.StackTrace)
                .HasMaxLength(10000);

            builder.Property(x => x.Path)
                .HasMaxLength(500);

            builder.Property(x => x.Method)
                .HasMaxLength(50);
        }
    }

}
