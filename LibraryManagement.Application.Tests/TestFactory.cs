using LibraryManagement.Domain.Entity;

namespace LibraryManagement.Application.Tests
{
    public static class TestFactory
    {
        public static Book Book(int quantity = 1)
        {
            var book = (Book)Activator.CreateInstance(typeof(Book), true)!;

            typeof(Book).GetProperty("Id")!.SetValue(book, 1);
            typeof(Book).GetProperty("Quantity")!.SetValue(book, quantity);

            return book;
        }

        public static Patron Patron()
        {
            var patron = (Patron)Activator.CreateInstance(typeof(Patron), true)!;

            typeof(Patron).GetProperty("Id")!.SetValue(patron, 1);

            return patron;
        }

        public static BorrowRecord BorrowRecord(int bookId, int patronId = 1)
        {
            return LibraryManagement.Domain.Entity.BorrowRecord.Create(bookId, patronId);
        }
    }
}
