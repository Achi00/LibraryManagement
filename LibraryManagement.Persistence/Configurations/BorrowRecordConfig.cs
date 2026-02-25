using LibraryManagement.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Persistence.Configurations
{
    public class BorrowRecordConfig : IEntityTypeConfiguration<BorrowRecord>
    {
        public void Configure(EntityTypeBuilder<BorrowRecord> builder)
        {
            builder.ToTable("BorrowRecords");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.BorrowDate)
                .IsRequired();

            builder.Property(x => x.DueDate)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired();

            // Relationship: BorrowRecord => Book
            builder.HasOne(x => x.Book)
                .WithMany()
                .HasForeignKey(x => x.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship: BorrowRecord => Patron
            builder.HasOne(x => x.Patron)
                .WithMany(p => p.BorrowRecords)
                .HasForeignKey(x => x.PatronId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.BookId);
            builder.HasIndex(x => x.PatronId);
            builder.HasIndex(x => x.DueDate);
        }
    }
}
