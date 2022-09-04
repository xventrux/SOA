using API.Contracts.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.AppServices.Services.BookServices
{
    public interface IBookService
    {
        Task AddAsync(BookCreateDto model);
    }
}
