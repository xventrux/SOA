using API.Infrastructure.FileSystem.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Infrastructure.FileSystem
{
    public class Storage : IStorage
    {
        private readonly string _webRootPath;
        public Storage(string webRootPath)
        {
            _webRootPath = webRootPath;
        }
        public async Task<FileData> UploadFileAsync(string path, IFormFile file)
        {
            if (file != null)
            {
                path = _webRootPath + $"/Files/" + path;

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path += file.FileName;

                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return new FileData() { Name = file.FileName, Path = path };
            }
            else
            {
                throw new Exception("Файл не найден");
            }
        }
    }
}
