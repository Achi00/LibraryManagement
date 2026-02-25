using LibraryManagement.Domain.Entity;
using LibraryManagement.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence
{
    public class SeedData
    {
        public static async Task SeedAsync(LibraryManagementContext context)
        {
            await context.Database.MigrateAsync();

            if (!context.Authors.Any())
            {
                var authors = new List<Author>
            {
                CreateAuthor("George", "Orwell", new DateTime(1903, 6, 25)),
                CreateAuthor("Jane", "Austen", new DateTime(1775, 12, 16)),
                CreateAuthor("Mark", "Twain", new DateTime(1835, 11, 30)),
                CreateAuthor("Fyodor", "Dostoevsky", new DateTime(1821, 11, 11)),
                CreateAuthor("J.K.", "Rowling", new DateTime(1965, 7, 31))
            };

                context.Authors.AddRange(authors);
                await context.SaveChangesAsync();
            }

            if (!context.Books.Any())
            {
                var authors = await context.Authors.ToListAsync();

                var books = new List<Book>
            {
                CreateBook("1984", "1111111111111", 1949, authors[0].Id, 5),
                CreateBook("Animal Farm", "2222222222222", 1945, authors[0].Id, 3),

                CreateBook("Pride and Prejudice", "3333333333333", 1813, authors[1].Id, 4),

                CreateBook("Adventures of Huckleberry Finn", "4444444444444", 1884, authors[2].Id, 2),

                CreateBook("Crime and Punishment", "5555555555555", 1866, authors[3].Id, 6),

                CreateBook("Harry Potter and the Sorcerer's Stone", "6666666666666", 1997, authors[4].Id, 10)
            };

                context.Books.AddRange(books);
                await context.SaveChangesAsync();
            }

            if (!context.Patrons.Any())
            {
                var patrons = new List<Patron>
            {
                CreatePatron("John", "Doe", "john@example.com"),
                CreatePatron("Jane", "Smith", "jane@example.com"),
                CreatePatron("Michael", "Brown", "michael@example.com")
            };

                context.Patrons.AddRange(patrons);
                await context.SaveChangesAsync();
            }
        }
        private static Author CreateAuthor(string first, string last, DateTime dob)
        {
            var author = (Author)Activator.CreateInstance(typeof(Author), true)!;

            typeof(Author).GetProperty("FirstName")!.SetValue(author, first);
            typeof(Author).GetProperty("LastName")!.SetValue(author, last);
            typeof(Author).GetProperty("DateOfBirth")!.SetValue(author, dob);

            return author;
        }

        private static Book CreateBook(string title, string isbn, int year, int authorId, int quantity)
        {
            var book = (Book)Activator.CreateInstance(typeof(Book), true)!;

            typeof(Book).GetProperty("Title")!.SetValue(book, title);
            typeof(Book).GetProperty("ISBN")!.SetValue(book, isbn);
            typeof(Book).GetProperty("PublicationYear")!.SetValue(book, year);
            typeof(Book).GetProperty("AuthorId")!.SetValue(book, authorId);
            typeof(Book).GetProperty("Quantity")!.SetValue(book, quantity);

            return book;
        }

        private static Patron CreatePatron(string first, string last, string email)
        {
            var patron = (Patron)Activator.CreateInstance(typeof(Patron), true)!;

            typeof(Patron).GetProperty("FirstName")!.SetValue(patron, first);
            typeof(Patron).GetProperty("LastName")!.SetValue(patron, last);
            typeof(Patron).GetProperty("Email")!.SetValue(patron, email);
            typeof(Patron).GetProperty("MembershipDate")!
                .SetValue(patron, DateTime.UtcNow);

            return patron;
        }
    }
}
