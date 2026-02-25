using LibraryManagement.Application.DTOs.BorrowRecord;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Entity;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;

namespace LibraryManagement.Application.Tests
{
    public class BorrowRecordServiceTests
    {
        private readonly Mock<IBorrowRecordRepository> _borrowRepo = new();
        private readonly Mock<IBookRepository> _bookRepo = new();
        private readonly Mock<IPatronRepository> _patronRepo = new();
        private readonly Mock<IUnitOfWork> _uow = new();

        private BorrowRecordService CreateService()
        {
            return new BorrowRecordService(
                _borrowRepo.Object,
                _bookRepo.Object,
                _patronRepo.Object,
                _uow.Object,
                Mock.Of<ILogger<BorrowRecordService>>()
            );
        }

        [Fact]
        public async Task BorrowAsync_Should_Decrease_Quantity_When_Successful()
        {
            // Arrange
            var book = TestFactory.Book(quantity: 2);
            var patron = TestFactory.Patron();

            _bookRepo.Setup(x => x.GetForUpdateAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(book);

            _patronRepo.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(patron);

            var service = CreateService();

            // Act
            var result = await service.BorrowAsync(
                new CreateBorrowRecordRequest { BookId = 1, PatronId = 1 },
                CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, book.Quantity);
        }

        [Fact]
        public async Task BorrowAsync_Should_Throw_When_Quantity_Is_Zero()
        {
            var book = TestFactory.Book(quantity: 0);
            var patron = TestFactory.Patron();

            _bookRepo.Setup(x => x.GetForUpdateAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(book);

            _patronRepo.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(patron);

            var transaction = new Mock<IDbContextTransaction>();

            _uow
                .Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(transaction.Object);

            var service = CreateService();

            await Assert.ThrowsAsync<BusinessRuleViolationException>(() =>
                service.BorrowAsync(
                    new CreateBorrowRecordRequest { BookId = 1, PatronId = 1 },
                    CancellationToken.None));
        }

        [Fact]
        public async Task BorrowAsync_Should_Throw_When_Book_Not_Found()
        {
            _bookRepo.Setup(x => x.GetForUpdateAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Book?)null);

            var transaction = new Mock<IDbContextTransaction>();
            _uow.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(transaction.Object);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() =>
                service.BorrowAsync(
                    new CreateBorrowRecordRequest { BookId = 1, PatronId = 1 },
                    CancellationToken.None));
        }

        [Fact]
        public async Task BorrowAsync_Should_Throw_When_Patron_Not_Found()
        {
            var book = TestFactory.Book(quantity: 2);

            _bookRepo.Setup(x => x.GetForUpdateAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(book);

            _patronRepo.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Patron?)null);

            var transaction = new Mock<IDbContextTransaction>();
            _uow.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(transaction.Object);

            var service = CreateService();

            await Assert.ThrowsAsync<NotFoundException>(() =>
                service.BorrowAsync(
                    new CreateBorrowRecordRequest { BookId = 1, PatronId = 1 },
                    CancellationToken.None));
        }

        [Fact]
        public async Task BorrowAsync_Should_Add_BorrowRecord_On_Success()
        {
            var book = TestFactory.Book(quantity: 2);
            var patron = TestFactory.Patron();

            _bookRepo.Setup(x => x.GetForUpdateAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(book);

            _patronRepo.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(patron);

            var transaction = new Mock<IDbContextTransaction>();
            _uow.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(transaction.Object);

            var service = CreateService();

            await service.BorrowAsync(
                new CreateBorrowRecordRequest { BookId = 1, PatronId = 1 },
                CancellationToken.None);

            _borrowRepo.Verify(x => x.Add(It.IsAny<BorrowRecord>()), Times.Once);
        }

        [Fact]
        public async Task BorrowAsync_Should_Call_SaveChanges_On_Success()
        {
            var book = TestFactory.Book(quantity: 2);
            var patron = TestFactory.Patron();

            _bookRepo.Setup(x => x.GetForUpdateAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(book);

            _patronRepo.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(patron);

            var transaction = new Mock<IDbContextTransaction>();
            _uow.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(transaction.Object);

            var service = CreateService();

            await service.BorrowAsync(
                new CreateBorrowRecordRequest { BookId = 1, PatronId = 1 },
                CancellationToken.None);

            _uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task BorrowAsync_Should_Rollback_When_Exception_Occurs()
        {
            var book = TestFactory.Book(quantity: 0);
            var patron = TestFactory.Patron();

            _bookRepo.Setup(x => x.GetForUpdateAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(book);

            _patronRepo.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(patron);

            var transaction = new Mock<IDbContextTransaction>();

            _uow.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(transaction.Object);

            var service = CreateService();

            await Assert.ThrowsAsync<BusinessRuleViolationException>(() =>
                service.BorrowAsync(
                    new CreateBorrowRecordRequest { BookId = 1, PatronId = 1 },
                    CancellationToken.None));

            transaction.Verify(
                t => t.RollbackAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

    }
}
