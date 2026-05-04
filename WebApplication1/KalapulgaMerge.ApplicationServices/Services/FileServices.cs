using KalapulgaMerge.Core.Domain;
using KalapulgaMerge.Core.Dto;
using KalapulgaMerge.Core.ServiceInterface;
using KalapulgaMerge.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace KalapulgaMerge.ApplicationServices.Services
{
    public class FilesServices : IFilesServices
    {
        private readonly IHostEnvironment _webHost;
        private readonly KalapulkDbContext _context;

        public FilesServices(IHostEnvironment webHost, KalapulkDbContext context)
        {
            _webHost = webHost;
            _context = context;
        }

        public void FilesToApi(ShopItemDTO dto, ShopItem domain)
        {
            if (dto.Files != null && dto.Files.Count > 0)
            {
                string uploadsFolder = Path.Combine(_webHost.ContentRootPath, "wwwroot", "multipleFileUpload");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var file in dto.Files)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    FileToApi path = new FileToApi
                    {
                        ImageID = Guid.NewGuid(),
                        FilePath = uniqueFileName,
                        ShopItemID = domain.Id,
                    };

                    _context.FilesToApi.Add(path);
                }
                _context.SaveChanges();
            }
        }



        public async Task<FileToApi> RemoveImageFromApi(FileToApiDTO dto)
        {
            var imageID = await _context.FilesToApi.FirstOrDefaultAsync(x => x.ImageID == dto.ImageID);
            
            if (imageID == null) return null;

            var filePath = _webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\" + imageID.FilePath;

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            _context.FilesToApi.Remove(imageID);
            await _context.SaveChangesAsync();

            return null;
        }

        public async Task<List<FileToApi>> RemoveImagesFromApi(FileToApiDTO[] dtos)
        {
            foreach (var dto in dtos)
            {
                await RemoveImageFromApi(dto);
            }
            return null;
        }
    }
}