using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KalapulgaMerge.Core.Domain;
using KalapulgaMerge.Core.Dto;

namespace KalapulgaMerge.Core.ServiceInterface
{
    public interface IFilesServices
    {
        void FilesToApi(ShopItemDTO dto, ShopItem domain);
        Task<FileToApi> RemoveImageFromApi(FileToApiDTO dto);
        Task<List<FileToApi>> RemoveImagesFromApi(FileToApiDTO[] dtos);
    }
}
