using API.Contracts.Book;
using API.Domain.Entities;
using API.Infrastructure.FileSystem;
using API.Infrastructure.FileSystem.Models;
using API.Infrastructure.Repository;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.AppServices.Services.BookServices
{
    public class BookService : IBookService
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IMapper _mapper;
        private readonly IStorage _storage;

        public BookService(IRepository<Book> bookRepository, 
            IMapper mapper, 
            IStorage storage)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _storage = storage;
        }

        public async Task AddAsync(BookCreateDto model)
        {
            Book book = _mapper.Map<Book>(model);
            book.Images = new List<MyFile>();

            foreach (var i in model.Images)
            {
                FileData file = await _storage.UploadFileAsync($"/Books/{book.Name}/", i);
                book.Images.Add(new MyFile()
                {
                    CreationDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    isDelete = false,
                    FileName = file.Name,
                    Path = file.Path
                });
            }


            await _bookRepository.AddAsync(book);
        }
    }
}
