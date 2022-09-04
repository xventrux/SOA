using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Domain.Entities.Base
{
    /// <summary>
    /// Базовая модель базы данных
    /// </summary>
    public class EntityBase
    {
        /// <summary>
        /// Идентификатор модели
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Дата создания модели
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Дата обновления модели
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Дата удаления модели
        /// </summary>
        public DateTime DeleteDate { get; set; }

        /// <summary>
        /// Удалена ли модель
        /// </summary>
        public bool isDelete { get; set; }
    }
}
