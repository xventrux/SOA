using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Contracts.Book
{
    /// <summary>
    /// Модель создания книги
    /// </summary>
    public class BookCreateDto
    {
        /// <summary>
        /// Название книги
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание книги
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Изображения книги
        /// </summary>
        public List<IFormFile> Images { get; set; }
    }
}
