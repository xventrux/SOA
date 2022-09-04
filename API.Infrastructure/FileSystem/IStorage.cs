using API.Infrastructure.FileSystem.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Infrastructure.FileSystem
{
    /// <summary>
    /// Сервис для управления файловой системой
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// Загрузить файл
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="file">Файл</param>
        public Task<FileData> UploadFileAsync(string path, IFormFile file);
    }
}
