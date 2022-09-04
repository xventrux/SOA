
using API.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Domain.Entities
{
    /// <summary>
    /// Книга
    /// </summary>
    public class Book : EntityBase
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
        public List<MyFile> Images { get; set; }
    }
}
