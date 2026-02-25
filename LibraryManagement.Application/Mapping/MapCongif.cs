using LibraryManagement.Application.DTOs.Author;
using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.DTOs.Patron;
using LibraryManagement.Domain.Entity;
using Mapster;

namespace LibraryManagement.Application.Mapping
{
    public class MapCongif : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Book, BookResponse>()
                .Map(dest => dest.AuthorName, src => $"{src.Author.FirstName} {src.Author.LastName}");

            config.NewConfig<Author, AuthorResponse>()
                .Map(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}");

            config.NewConfig<Patron, PatronResponse>()
                .Map(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}");
        }
    }
}
