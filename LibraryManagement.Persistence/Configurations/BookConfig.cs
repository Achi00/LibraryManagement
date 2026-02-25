using LibraryManagement.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Persistence.Configurations
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
               .IsRequired()
               .HasMaxLength(200);

            builder.Property(x => x.ISBN)
               .IsRequired()
               .HasMaxLength(13);

            builder.Property(x => x.Quantity)
               .HasDefaultValue(0);

            builder.Property(x => x.CoverImageUrl)
            .HasMaxLength(500);

            builder.Property(x => x.Description)
            .HasMaxLength(2000);

            // for sorting by year
            builder.HasIndex(x => x.PublicationYear);
            builder.HasIndex(x => x.AuthorId);
            builder.HasIndex(x => x.Title);
            builder.HasIndex(x => x.ISBN).IsUnique();

            // relationships
            builder.HasOne(x => x.Author)
                .WithMany(x => x.Books)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
